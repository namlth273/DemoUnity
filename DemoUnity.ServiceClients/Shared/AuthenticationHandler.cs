using DemoUnity.ServiceClients.Abstractions.Services;
using DemoUnity.ServiceClients.Abstractions.Shared;
using Microsoft.Extensions.Caching.Memory;
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

        public AuthenticationHandler(ISecurityTokenAccessor securityTokenAccessor, IPolicyFactory policyFactory)
        {
            _securityTokenAccessor = securityTokenAccessor;

            // Create a policy that tries to renew the access token if a 403 Unauthorized is received.
            _policy = policyFactory.CreateExceptionPolicy(_securityTokenAccessor.RenewAccessTokenAsync()).WrapAsync(
                policyFactory.CreateRetryPolicy(_securityTokenAccessor.RenewAccessTokenAsync()));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Try to perform the request, re-authenticating gracefully if the call fails due to an expired or revoked access token.
            var result = await _policy.ExecuteAndCaptureAsync(async () =>
            {
                var token = await _securityTokenAccessor.GetAccessTokenFromCacheAsync();
                var authenticationHeader = new AuthenticationHeaderValue("Bearer", token);
                request.Headers.Authorization = authenticationHeader;
                return await base.SendAsync(request, cancellationToken);
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
    }

    public class VestorlyAuthenticationHandler : DelegatingHandler
    {
        private readonly int _retryCount = 1;
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;
        private readonly IVestorlySecurityTokenAccessor _securityTokenAccessor;

        public VestorlyAuthenticationHandler(IVestorlySecurityTokenAccessor securityTokenAccessor,
            IPolicyFactory policyFactory)
        {
            _securityTokenAccessor = securityTokenAccessor;

            // Create a policy that tries to renew the access token if a 403 Unauthorized is received.
            _policy = policyFactory.CreateExceptionPolicy(_securityTokenAccessor.RenewAccessTokenAsync()).WrapAsync(
                policyFactory.CreateRetryPolicy(_securityTokenAccessor.RenewAccessTokenAsync()));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Try to perform the request, re-authenticating gracefully if the call fails due to an expired or revoked access token.
            var result = await _policy.ExecuteAndCaptureAsync(async () =>
            {
                var token = await _securityTokenAccessor.GetAccessTokenFromCacheAsync();

                request.Headers.Add("x-vestorly-auth", token);

                return await base.SendAsync(request, cancellationToken);
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
    }

    public class VestorlySecurityTokenAccessor : IVestorlySecurityTokenAccessor
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IVestorlyAuthenticationServiceClient _authenticationServiceClient;
        private const string AccessTokenKey = "Vestorly.MasterAccessToken";

        public VestorlySecurityTokenAccessor(IMemoryCache memoryCache, IVestorlyAuthenticationServiceClient authenticationServiceClient)
        {
            _memoryCache = memoryCache;
            _authenticationServiceClient = authenticationServiceClient;
        }

        public Task<string> GetAccessTokenFromCacheAsync()
        {
            _memoryCache.TryGetValue(AccessTokenKey, out string accessToken);
            return Task.FromResult(accessToken);
        }

        public async Task<AccessToken> RenewAccessTokenAsync()
        {
            var accessToken = await _authenticationServiceClient.SignInAsMasterAccount();

            _memoryCache.Set(AccessTokenKey, accessToken.AccessToken);

            return new AccessToken
            {
                Token = accessToken.AccessToken
            };
        }
    }
}