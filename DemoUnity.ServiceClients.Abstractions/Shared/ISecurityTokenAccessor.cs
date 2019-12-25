using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Abstractions.Shared
{
    public interface ISecurityTokenAccessor
    {
        Task<string> GetAccessTokenFromCacheAsync();
        Task<AccessToken> RenewAccessTokenAsync();
    }
}