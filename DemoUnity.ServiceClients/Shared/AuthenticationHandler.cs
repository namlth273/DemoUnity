using DemoUnity.ServiceClients.Abstractions.Shared;
using Polly;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Shared
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly int _retryCount = 1;
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;
        private readonly ISecurityTokenAccessor _securityTokenAccessor;
        private AuthenticationHeaderValue _authenticationHeader;
        private IAccessToken _accessToken;

        public AuthenticationHandler(ISecurityTokenAccessor securityTokenAccessor, IPolicyFactory policyFactory)
        {
            _securityTokenAccessor = securityTokenAccessor;

            // Create a policy that tries to renew the access token if a 403 Unauthorized is received.
            _policy = policyFactory.CreateRetryPolicy(AuthenticateAsync());
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Request an access token if we don't have one yet or if it has expired.
            if (request.Headers.Authorization == null)
            {
                _accessToken = new AccessToken
                {
                    Token = await _securityTokenAccessor.GetAccessTokenFromCacheAsync(),
                };
                _authenticationHeader = new AuthenticationHeaderValue("Bearer", _accessToken.Token);
            }

            // Try to perform the request, re-authenticating gracefully if the call fails due to an expired or revoked access token.
            var result = await _policy.ExecuteAndCaptureAsync(() =>
            {
                request.Headers.Authorization = _authenticationHeader;
                return base.SendAsync(request, cancellationToken);
            });

            // Handle HTTP response
            if (result.Result == null)
            {
                throw new UnauthorizedException(
                    $"ReasonPhrase: {result.FinalHandledResult?.ReasonPhrase}. " +
                    $"FinalException: {result.FinalException}. " +
                    $"AccessToken is still invalid after trying refresh {_retryCount} time(s).",
                    request);
            }

            return result.Result;
        }

        /// <summary>
        /// Renew access token and set to AuthenticationHeader object.
        /// </summary>
        /// <returns></returns>
        private async Task AuthenticateAsync()
        {
            _accessToken = await _securityTokenAccessor.RenewAccessTokenAsync();
            _authenticationHeader = new AuthenticationHeaderValue("Bearer", _accessToken.Token);
        }
    }
}