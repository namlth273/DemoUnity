using DemoUnity.ServiceClients.Abstractions.Shared;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Shared
{
    public class SecurityTokenDecorator : ISecurityTokenAccessor
    {
        private readonly ISecurityTokenAccessor _securityTokenAccessor;

        public SecurityTokenDecorator(ISecurityTokenAccessor securityTokenAccessor)
        {
            _securityTokenAccessor = securityTokenAccessor;
        }

        public async Task<string> GetAccessTokenFromCacheAsync()
        {
            var accessToken = await _securityTokenAccessor.GetAccessTokenFromCacheAsync();

            if (string.IsNullOrEmpty(accessToken))
                throw new AccessTokenNotFoundException();

            return accessToken;
        }

        public Task<AccessToken> RenewAccessTokenAsync()
        {
            return _securityTokenAccessor.RenewAccessTokenAsync();
        }
    }
}