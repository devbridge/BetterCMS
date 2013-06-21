using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;

using Autofac;

using BetterCms.Api;
using BetterCms.Core.Modules;
using BetterCms.Core.Mvc.Extensions;

namespace BetterCms.Module.WebApi
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class WebApiModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "api";

        /// <summary>
        /// The API area name.
        /// </summary>
        internal const string WebApiAreaName = "bcms-api";

        /// <summary>
        /// Controller extensions.
        /// </summary>
        private readonly IControllerExtensions controllerExtensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        public WebApiModuleDescriptor(ICmsConfiguration cmsConfiguration, IControllerExtensions controllerExtensions)
            : base(cmsConfiguration)
        {
            this.controllerExtensions = controllerExtensions;
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of API module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "A Web API module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                return WebApiAreaName;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<BlogsApiContext>().AsSelf().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);

            RegisterModuleControllers(containerBuilder);
        }

        /// <summary>
        /// Registers module custom routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            GlobalConfiguration.Configuration.EnableQuerySupport();

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "BcmsWebApiRoutesREST",
                routeTemplate: "bcms-api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, area = AreaName }
            );

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "BcmsWebApiRoutesRPC",
                routeTemplate: "bcms-api/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional, area = AreaName }
            );
        }

        /// <summary>
        /// Registers the module controllers.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        private void RegisterModuleControllers(ContainerBuilder containerBuilder)
        {
            var controllerTypes = controllerExtensions.GetControllerTypes<IHttpController>(Assembly.GetExecutingAssembly());

            if (controllerTypes != null)
            {
                foreach (Type controllerType in controllerTypes)
                {
                    RegisterKeyedType<IHttpController>(containerBuilder, controllerType);
                }
            }
        }
    }
}
