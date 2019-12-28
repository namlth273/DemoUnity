using DemoUnity.ServiceClients.Abstractions.Shared;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Shared
{
    public class SecurityTokenAccessor : ISecurityTokenAccessor
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string AccessTokenKey = "DemoUnity.AccessToken";

        public SecurityTokenAccessor(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public Task<string> GetAccessTokenFromCacheAsync()
        {
            _memoryCache.TryGetValue(AccessTokenKey, out string accessToken);
            return Task.FromResult(accessToken);
        }

        public async Task<AccessToken> RenewAccessTokenAsync()
        {
            var tokenResponse = await _httpClientFactory.CreateClient().RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "http://localhost:10000/connect/token",
                ClientId = "client",
                ClientSecret = "secret",

                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            _memoryCache.Set(AccessTokenKey, tokenResponse.AccessToken);

            return new AccessToken
            {
                Token = tokenResponse.AccessToken
            };
        }
    }
}