using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;

namespace DemoUnity.WebAutofac
{
    public class IoCConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(IoCConfig).Assembly);
            builder.RegisterModule<ServiceClients.Autofac.Commons.AutofacModule>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}