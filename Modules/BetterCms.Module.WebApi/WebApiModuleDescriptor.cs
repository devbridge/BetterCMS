using System.Web.Http;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Modules;

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
        /// Initializes a new instance of the <see cref="WebApiModuleDescriptor" /> class.
        /// </summary>
        public WebApiModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
            ApiContext.Events.HostStart += Events_HostStart;
        }

        /// <summary>
        /// Events occurs on host start.
        /// </summary>
        /// <param name="args">The args.</param>
        private void Events_HostStart(SingleItemEventArgs<System.Web.HttpApplication> args)
        {
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

        public override void RegisterCustomRoutes(ModuleRegistrationContext context, Autofac.ContainerBuilder containerBuilder)
        {
            GlobalConfiguration.Configuration.EnableQuerySupport();
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: "BcmsWebApiRoutes",
                routeTemplate: "bcms-api/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, area = "bcms-api" }
            );
        }
    }
}
