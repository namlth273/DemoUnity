using DemoUnity.ServiceClients.Abstractions.Shared;
using Polly;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoUnity.ServiceClients.Shared
{
    public class PolicyFactory : IPolicyFactory
    {
        public IAsyncPolicy CreateExceptionPolicy(Task delegatingAction, int retryCount = 1)
        {
            return Policy.Handle<AccessTokenNotFoundException>()
                .RetryAsync(retryCount, async (result, count, context) => await delegatingAction);
        }

        public IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(Task delegatingAction, int retryCount = 1)
        {
            return Policy
                .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(retryCount, async (result, count, context) => await delegatingAction);
        }
    }

    public class NoOpPolicyFactory : IPolicyFactory
    {
        public IAsyncPolicy CreateExceptionPolicy(Task delegatingAction, int retryCount = 1)
        {
            return Policy.NoOpAsync();
        }

        public IAsyncPolicy<HttpResponseMessage> CreateRetryPolicy(Task delegatingAction, int retryCount = 1)
        {
            return Policy.NoOpAsync<HttpResponseMessage>();
        }
    }
}