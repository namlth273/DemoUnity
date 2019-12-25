using Autofac;
using Autofac.Extensions.DependencyInjection;
using DemoUnity.ServiceClients.Abstractions.Services;
using DemoUnity.ServiceClients.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DemoUnity.ServiceClients.Autofac.Commons
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddHttpClient<ITestServiceClient, TestServiceClient>(client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            });

            builder.Populate(services);
        }
    }
}