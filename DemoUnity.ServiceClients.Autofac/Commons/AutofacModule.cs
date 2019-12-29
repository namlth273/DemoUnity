using Autofac;
using Autofac.Extensions.DependencyInjection;
using DemoUnity.ServiceClients.Abstractions.Services;
using DemoUnity.ServiceClients.Abstractions.Shared;
using DemoUnity.ServiceClients.Services;
using DemoUnity.ServiceClients.Shared;
using Microsoft.Extensions.Caching.Memory;
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
                    client.BaseAddress = new Uri("https://localhost:44351");
                })
                .AddHttpMessageHandler<AuthenticationHandler>();

            builder.Populate(services);

            builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();
            builder.RegisterType<SecurityTokenAccessor>().As<ISecurityTokenAccessor>();
            builder.RegisterDecorator<SecurityTokenDecorator, ISecurityTokenAccessor>();
            builder.RegisterType<AuthenticationHandler>();
            builder.RegisterType<PolicyFactory>().As<IPolicyFactory>();
            //builder.RegisterType<NoOpPolicyFactory>().As<IPolicyFactory>();

        }
    }
}