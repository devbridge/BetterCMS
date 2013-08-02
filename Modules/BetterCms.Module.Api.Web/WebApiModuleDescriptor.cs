using System.Web;

using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules;
using BetterCms.Events;

namespace BetterCms.Module.Api
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class WebApiModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "web-api";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public WebApiModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
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
                return "An Web API module for Better CMS.";
            }
        }
       
        /// <summary>
        /// Registers module custom routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            context.IgnoreRoute("bcms-api/{*pathInfo}");
        }

        private void ApplicationStart(SingleItemEventArgs<HttpApplication> args)
        {
            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var containerProvider = container.Resolve<PerWebRequestContainerProvider>();
                new WebApiApplicationHost(() => containerProvider.CurrentScope).Init();
            }
        }
    }
}
