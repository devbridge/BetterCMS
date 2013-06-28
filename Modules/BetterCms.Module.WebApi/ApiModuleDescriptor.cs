using System.Web;

using Autofac;

using BetterCms.Api;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;
using BetterCms.Events;

namespace BetterCms.Module.Api
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class ApiModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "api";

        /// <summary>
        /// The API area name.
        /// </summary>
        internal const string ApiAreaName = "bcms-api";

        /// <summary>
        /// Controller extensions.
        /// </summary>
        private readonly IControllerExtensions controllerExtensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        public ApiModuleDescriptor(ICmsConfiguration cmsConfiguration, IControllerExtensions controllerExtensions)
            : base(cmsConfiguration)
        {
            this.controllerExtensions = controllerExtensions;
            CoreEvents.Instance.HostStart += ApplicationStart;
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
                return "An API module for Better CMS.";
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
                return ApiAreaName;
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
            containerBuilder.RegisterType<LayoutService>().As<ILayoutService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<LayoutRegionsService>().As<ILayoutRegionService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
        }

        /// <summary>
        /// Registers module custom routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            context.IgnoreRoute(string.Format("{0}/{{*pathInfo}}", ApiAreaName));
        }

        private void ApplicationStart(SingleItemEventArgs<HttpApplication> args)
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var containerProvider = container.Resolve<PerWebRequestContainerProvider>();                
                new ApiApplicationHost(() => containerProvider.CurrentScope).Init();
            }
        }
    }
}
