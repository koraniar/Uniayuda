using Uniayuda.Infraestructure;
using Autofac;
using Logic.Interfaces;
using Owin;
using Cross;

namespace Uniayuda
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AutoMapperConfiguration.ConfigureMapper();
            IoCConfig.RegisterDependencies(app);

            using (var scope = IoCConfig.ApplicationContainer.Container.BeginLifetimeScope(Constants.AutofacWebRequest))
            {
                IDatabaseInitializer dbIntializer = scope.Resolve<IDatabaseInitializer>();
                dbIntializer.Initialize();
            }

            ConfigureAuth(app);
        }
    }
}