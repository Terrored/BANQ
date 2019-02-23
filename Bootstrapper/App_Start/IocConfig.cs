﻿using Autofac;
using Autofac.Integration.Mvc;
using DataAccess;
using System.Data.Entity;
using System.Web.Mvc;

using WebLibrary;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Bootstrapper.IocConfig), "RegisterDependencies")]

namespace Bootstrapper
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            const string nameOrConnectionString = "name=BANQConnectionString";
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.Register<DbContext>(b =>
            {

                var context = new ApplicationDbContext();
                return context;
            }).InstancePerHttpRequest();

            builder.RegisterModule(new IdentityModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}