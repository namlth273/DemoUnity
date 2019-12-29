using Polly;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Abstractions.Shared
{
    public interface IAccessToken
    {
        string Token { get; set; }
    }

    public class AccessToken : IAccessToken
    {
        public string Token { get; set; }
    }

    public interface IPolicyFactory
    {
        IAsyncPolicy CreateExceptionPolicy(Task delegatingAction, int retryCount = 1);
        IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(Task delegatingAction, int retryCount = 1);
    }
}