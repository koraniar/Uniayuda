using Autofac;
using Autofac.Integration.Mvc;
using IoC;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Reflection;
using System.Web.Mvc;

namespace Uniayuda.Infraestructure
{
    public class IoCConfig
    {
        public static void RegisterDependencies(IAppBuilder app)
        {
            var builder = BaseIoCConfig.GetBaseBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterInstance(app.GetDataProtectionProvider()).As<IDataProtectionProvider>();
            var container = builder.Build();

            ApplicationContainer.Container = container;
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        //Static class for store the Container if we need a manual resolve
        public static class ApplicationContainer
        {
            public static IContainer Container { get; set; }
        }
    }
}