using System;
using System.Web;

using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules;
using BetterCms.Events;

namespace BetterCms.Module.Api
{
    using Common.Logging;

    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class WebApiModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

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

        internal const string ModuleId = "f19e11dc-f991-48e7-be82-ab4d2c07209d";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public override Guid Id
        {
            get
            {
                return new Guid(ModuleId);
            }
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
            Logger.Info("OnHostStart: preparing web api...");

            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                var containerProvider = container.Resolve<PerWebRequestContainerProvider>();
                new WebApiApplicationHost(() => containerProvider.CurrentScope).Init();
            }

            Logger.Info("OnHostStart: preparing web api completed.");
        }
    }
}
