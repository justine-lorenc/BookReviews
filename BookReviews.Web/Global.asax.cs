using Autofac;
using AutoMapper;
using BookReviews.Impl.Configuration;
using BookReviews.Impl.Logic;
using BookReviews.Impl.Logic.Interfaces;
using BookReviews.Impl.Repositories;
using BookReviews.Impl.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterDependencies();
            RegisterAutoMapper();
        }

        protected void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<BookLogic>().As<IBookLogic>();
            builder.RegisterType<ReviewLogic>().As<IReviewLogic>();

            builder.RegisterType<BookRepository>().As<IBookRepository>();
            builder.RegisterType<ReviewRepository>().As<IReviewRepository>();

            Container = builder.Build();
        }

        protected void RegisterAutoMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MapperProfile>();
            });
        }
    }
}
