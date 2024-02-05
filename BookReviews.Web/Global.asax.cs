using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using BookReviews.Impl;
using BookReviews.Impl.Logic;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Repositories;
using BookReviews.Impl.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookReviews.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Autofac.IContainer Container { get; set; }

        protected void Application_Start()
        {
            RegisterDependencies();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(ctx =>
            {
                var services = new ServiceCollection();
                services.AddHttpClient(Globals.AppSettings.BookSearchClientName, c =>
                {
                    c.BaseAddress = new Uri(Globals.AppSettings.GoogleBooksApiUrl);
                    c.Timeout = TimeSpan.FromMinutes(2);
                    c.DefaultRequestHeaders.Add("accept", "application/json");
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(4));

                var provider = services.BuildServiceProvider();
                return provider.GetRequiredService<IHttpClientFactory>();

            }).SingleInstance();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication)
                .As<IAuthenticationManager>();

            builder.RegisterType<BookLogic>().As<IBookLogic>();
            builder.RegisterType<ReviewLogic>().As<IReviewLogic>();
            builder.RegisterType<UserLogic>().As<IUserLogic>();

            builder.RegisterType<BookRepository>().As<IBookRepository>();
            builder.RegisterType<ReviewRepository>().As<IReviewRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();

            Assembly[] assemblies = new Assembly[] {
                typeof(BookReviews.Impl.Configuration.MapperProfile).Assembly,
                typeof(BookReviews.Web.Configuration.MapperProfile).Assembly
            };

            builder.RegisterAutoMapper(assemblies: assemblies);

            Container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));

            // this can be used to verify mapping configuration
            //var mapperConfiguration = Container.Resolve<MapperConfiguration>();
            //mapperConfiguration.AssertConfigurationIsValid();
        }
    }
}
