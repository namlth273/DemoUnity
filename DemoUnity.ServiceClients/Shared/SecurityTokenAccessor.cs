using DemoUnity.ServiceClients.Abstractions.Shared;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Shared
{
    public class SecurityTokenAccessor : ISecurityTokenAccessor
    {
        private readonly IMemoryCache _memoryCache;
        private const string AccessTokenKey = "AcessToken";

        public SecurityTokenAccessor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<string> GetAccessTokenFromCacheAsync()
        {
            _memoryCache.TryGetValue(AccessTokenKey, out string accessToken);
            return Task.FromResult(accessToken);
        }

        public Task<AccessToken> RenewAccessTokenAsync()
        {
            _memoryCache.Set(AccessTokenKey, Guid.NewGuid().ToString());
            return Task.FromResult(new AccessToken
            {
                Token = Guid.NewGuid().ToString()
            });
        }
    }
}