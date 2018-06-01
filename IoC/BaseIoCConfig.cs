using Autofac;
using Data.DBInteractions;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Logic.Interfaces;
using Logic.Services;

namespace IoC
{
    public class BaseIoCConfig
    {
        public static ContainerBuilder GetBaseBuilder()
        {
            var builder = new ContainerBuilder();

            //Db interactions
            builder.RegisterType<DBFactory>().As<IDBFactory>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            //Database repositories
            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<DatabaseInitializer>().As<IDatabaseInitializer>();
            builder.RegisterType<EmailRepository>().As<IEmailRepository>();

            //Services
            builder.RegisterType<DatabaseService>().As<IDatabaseService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<PostService>().As<IPostService>();
            builder.RegisterType<AssessmentService>().As<IAssessmentService>();
            builder.RegisterType<CommentService>().As<ICommentService>();

            return builder;
        }
    }
}
