﻿using Autofac;
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
                    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
                })
                .AddHttpMessageHandler<AuthenticationHandler>();

            builder.Populate(services);

            builder.RegisterType<MemoryCache>().As<IMemoryCache>();
            builder.RegisterType<SecurityTokenAccessor>().As<ISecurityTokenAccessor>();
            builder.RegisterDecorator<SecurityTokenDecorator, ISecurityTokenAccessor>();
            builder.RegisterType<AuthenticationHandler>();

        }
    }
}