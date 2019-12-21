using DemoUnity.ServiceClients.Abstractions.Services;
using DemoUnity.ServiceClients.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace DemoUnity.ServiceClients.Unity.Commons
{
    public static class IoCConfig
    {
        public static void RegisterTestServiceClient(this IServiceCollection services)
        {
            services.AddHttpClient<ITestServiceClient, TestServiceClient>(client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            });
        }

        public static IUnityContainer PopulateToUnityContainer(IUnityContainer unityContainer)
        {
            var services = new ServiceCollection();

            services.RegisterTestServiceClient();

            services.BuildServiceProvider(unityContainer);

            return unityContainer;
        }
    }
}