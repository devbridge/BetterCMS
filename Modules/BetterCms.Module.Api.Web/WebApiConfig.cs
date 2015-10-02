using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.ValueProviders;

using Autofac;
using Autofac.Integration.WebApi;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.Dependencies;

namespace BetterCms.Module.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "bcms-api/{controller}"
            //);

            config.Filters.Add(new ExceptionAttribute());
            config.Formatters.RemoveAt(0);
            config.Formatters.Insert(0, new ServiceStackTextFormatter());
            var bla = ContextScopeProvider.CreateChildContainer();
            var resolver = new AutofacWebApiDependencyResolver(ContextScopeProvider.CreateChildContainer());

            config.DependencyResolver = resolver;
        }
    }

}