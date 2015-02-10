using System.Web.Routing;

using Autofac;

using Devbridge.Platform.Core.Environment.Assemblies;
using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Web.Mvc.Extensions;

namespace Devbridge.Platform.Core.Web.Modules.Registration
{
    /// <summary>
    /// Default modules registration implementation.
    /// </summary>
    public class DefaultWebModulesRegistration : DefaultModulesRegistration, IWebModulesRegistration
    {
        /// <summary>
        /// The controller extensions
        /// </summary>
        private readonly IControllerExtensions controllerExtensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebModulesRegistration" /> class.
        /// </summary>
        /// <param name="assemblyLoader">The assembly loader.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        public DefaultWebModulesRegistration(IAssemblyLoader assemblyLoader, IControllerExtensions controllerExtensions)
            : base(assemblyLoader)
        {
            this.controllerExtensions = controllerExtensions;
        }

        /// <summary>
        /// Finds the module by area name.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <returns>Known module instance.</returns>
        public WebModuleDescriptor FindModuleByAreaName(string areaName)
        {
            ModuleRegistrationContext module;
            if (knownModules.TryGetValue(areaName.ToLowerInvariant(), out module))
            {
                return module.ModuleDescriptor as WebModuleDescriptor;
            }

            return null;
        }

        /// <summary>
        /// Determines whether module is registered by area name.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <returns>
        ///   <c>true</c> if module by area name is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsModuleRegisteredByAreaName(string areaName)
        {
            return knownModules.ContainsKey(areaName.ToLowerInvariant());
        }

        /// <summary>
        /// Register all modules routes
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterKnownModuleRoutes(RouteCollection routes)
        {
            foreach (var context in knownModules)
            {
                var webModuleContext = context.Value as WebModuleRegistrationContext;
                if (webModuleContext != null)
                {
                    foreach (var moduleRoute in webModuleContext.Routes)
                    {
                        routes.Add(moduleRoute);
                    }
                }
            }
        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="registrationContext">The registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        protected override void RegisterModuleDescriptor(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder)
        {
            var webContext = registrationContext as WebModuleRegistrationContext;
            if (webContext != null)
            {
                var webDescriptor = (WebModuleDescriptor)webContext.ModuleDescriptor;
                webDescriptor.RegisterModuleCommands(webContext, containerBuilder);
                webDescriptor.RegisterModuleControllers(webContext, containerBuilder, controllerExtensions);
                webDescriptor.RegisterCustomRoutes(webContext, containerBuilder);
            }

            base.RegisterModuleDescriptor(registrationContext, containerBuilder);
        }
    }
}
