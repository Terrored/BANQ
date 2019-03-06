using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using BusinessLogic;
using BusinessLogic.Interfaces;
using DataAccess;
using Model.RepositoryInterfaces;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using WebLibrary;
using WebLibrary.Controllers.Api;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Bootstrapper.IocConfig), "RegisterDependencies")]

namespace Bootstrapper
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MoneyTransferController).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();


            builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IEntityRepository<>));
            builder.RegisterType<BankAccountService>().As<IBankAccountService>();
            builder.RegisterType<MoneyTransferService>().As<IMoneyTransferService>();
            builder.RegisterType<BankAccountTypeService>().As<IBankAccountTypeService>();
            builder.Register<DbContext>(b =>
            {

                var context = new ApplicationDbContext();
                return context;
            }).InstancePerRequest();

            builder.RegisterModule(new IdentityModule());

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
