using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Web.Routing;

using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Core.Mvc.Routes;

using Common.Logging;

namespace BetterCms.Core.Modules.Registration
{
    /// <summary>
    /// Default modules registration implementation.
    /// </summary>
    public class DefaultModulesRegistration : IModulesRegistration
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Assembly loader instance.
        /// </summary>
        private readonly IAssemblyLoader assemblyLoader;

        /// <summary>
        /// CMS configuration instance.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Controller extensions.
        /// </summary>
        private readonly IControllerExtensions controllerExtensions;

        /// <summary>
        /// Thread safe known module types.
        /// </summary>
        private readonly Dictionary<string, Type> knownModuleDescriptorTypes;

        /// <summary>
        /// Thread safe modules dictionary.
        /// </summary>
        private readonly Dictionary<string, ModuleRegistrationContext> knownModules;

        /// <summary>
        /// Thread safe list of known java script modules dictionary.
        /// </summary>
        private readonly Dictionary<string, JsIncludeDescriptor> knownJavaScriptModules;

        /// <summary>
        /// Thread safe list of registered action projections for a sidebar main content.
        /// </summary>
        private readonly List<IPageActionProjection> knownSidebarBodyContentItems;

        /// <summary>
        /// Thread safe list of registered action projections for a sidebar left content.
        /// </summary>
        private readonly List<IPageActionProjection> knownSidebarContentItems;

        /// <summary>
        /// Thread safe list of registered action projections for a sidebar left content.
        /// </summary>
        private readonly List<IPageActionProjection> knownSidebarHeadContentItems;

        /// <summary>
        /// Thread safe list of registered action projections for a sidebar left content.
        /// </summary>
        private readonly List<IPageActionProjection> knownSiteSettingsItems;

        /// <summary>
        /// Thread safe style sheet files collection.
        /// </summary>
        private readonly List<CssIncludeDescriptor> knownStyleSheetIncludes;        

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultModulesRegistration" /> class.
        /// </summary>
        /// <param name="assemblyLoader">The assembly loader.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        /// <param name="routeTable">The route table.</param>
        public DefaultModulesRegistration(IAssemblyLoader assemblyLoader,  ICmsConfiguration cmsConfiguration, IControllerExtensions controllerExtensions, IRouteTable routeTable)
        {
            this.assemblyLoader = assemblyLoader;
            this.cmsConfiguration = cmsConfiguration;
            this.controllerExtensions = controllerExtensions;

            knownModuleDescriptorTypes = new Dictionary<string, Type>();
            knownModules = new Dictionary<string, ModuleRegistrationContext>();
            knownJavaScriptModules = new Dictionary<string, JsIncludeDescriptor>();
            knownSidebarHeadContentItems = new List<IPageActionProjection>();
            knownSidebarContentItems = new List<IPageActionProjection>();
            knownSidebarBodyContentItems = new List<IPageActionProjection>();
            knownSiteSettingsItems = new List<IPageActionProjection>();
            knownStyleSheetIncludes = new List<CssIncludeDescriptor>();            
        }

        /// <summary>
        /// Tries to scan and adds module descriptor type from assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        public void AddModuleDescriptorTypeFromAssembly(Assembly assembly)
        {
            if (Log.IsTraceEnabled)
            {
                Log.TraceFormat("Searching for module descriptor type in the assembly {0}.", assembly.FullName);
            }
      
            var moduleRegistrationType = assemblyLoader.GetLoadableTypes(assembly).Where(IsModuleDescriptorType).FirstOrDefault();
            if (moduleRegistrationType != null)
            {
                if (Log.IsTraceEnabled)
                {
                    Log.TraceFormat("Adds module descriptor {0} from the assembly {1}.", moduleRegistrationType.Name, assembly.FullName);
                }

                if (!knownModuleDescriptorTypes.ContainsKey(moduleRegistrationType.Name))
                {
                    knownModuleDescriptorTypes.Add(moduleRegistrationType.Name, moduleRegistrationType);
                }
                else
                {
                    Log.InfoFormat("Module descriptor {0} from the assembly {1} already included.", moduleRegistrationType.Name, assembly.FullName);
                }
            }
        }

        /// <summary>
        /// Gets registered modules.
        /// </summary>
        /// <returns>List of registered modules.</returns>
        public IEnumerable<ModuleRegistrationContext> GetModules()
        {
            return knownModules.Values;
        }

        /// <summary>
        /// Gets all known JS modules.
        /// </summary>
        /// <returns>Enumerator of known JS modules.</returns>
        public IEnumerable<JsIncludeDescriptor> GetJavaScriptModules()
        {
            return knownJavaScriptModules.Values;
        }

        /// <summary>
        /// Gets all client side actions from JS modules.
        /// </summary>
        /// <returns>Enumerator of known JS modules actions.</returns>
        public IEnumerable<IPageActionProjection> GetSidebarHeaderProjections()
        {
            return knownSidebarHeadContentItems;
        }

        /// <summary>
        /// Gets action projections to render in the sidebar left side.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar left side.</returns>
        public IEnumerable<IPageActionProjection> GetSidebarSideProjections()
        {
            return knownSidebarContentItems;
        }

        /// <summary>
        /// Gets action projections to render in the sidebar main container.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar main container.</returns>
        public IEnumerable<IPageActionProjection> GetSidebarBodyProjections()
        {
            return knownSidebarBodyContentItems;
        }

        /// <summary>
        /// Gets action projections to render in the site settings menu container.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the site settings container.</returns>
        public IEnumerable<IPageActionProjection> GetSiteSettingsProjections()
        {
            return knownSiteSettingsItems;
        }

        /// <summary>
        /// Gets known StyleSheet includes.
        /// </summary>
        /// <returns>Enumerator of known StyleSheet includes.</returns>
        public IEnumerable<CssIncludeDescriptor> GetStyleSheetIncludes()
        {
            return knownStyleSheetIncludes;
        }

        /// <summary>
        /// Finds the module by area name.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <returns>Known module instance.</returns>
        public ModuleDescriptor FindModuleByAreaName(string areaName)
        {
            ModuleRegistrationContext module;
            if (knownModules.TryGetValue(areaName.ToLowerInvariant(), out module))
            {
                return module.ModuleDescriptor;
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
        /// Registers Better CMS module.
        /// </summary>
        /// <param name="moduleDescriptor">Module information.</param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public void RegisterModule(ModuleDescriptor moduleDescriptor)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            ModuleRegistrationContext registrationContext = new ModuleRegistrationContext(moduleDescriptor);            
           
            moduleDescriptor.RegisterModuleTypes(registrationContext, containerBuilder);            
            moduleDescriptor.RegisterModuleCommands(registrationContext, containerBuilder);            
            moduleDescriptor.RegisterModuleControllers(registrationContext, containerBuilder, controllerExtensions);
            moduleDescriptor.RegisterCustomRoutes(registrationContext, containerBuilder);

            ContextScopeProvider.RegisterTypes(containerBuilder);

            knownModules.Add(moduleDescriptor.AreaName.ToLowerInvariant(), registrationContext);
            
            var jsModules = moduleDescriptor.RegisterJsIncludes();            
            if (jsModules != null)
            {
                foreach (var jsModuleDescriptor in jsModules)
                {
                    knownJavaScriptModules.Add(jsModuleDescriptor.Name, jsModuleDescriptor);
                }
            }

            var sidebarHeadProjections = moduleDescriptor.RegisterSidebarHeaderProjections(containerBuilder);
            UpdateConcurrentBagWithEnumerator(knownSidebarHeadContentItems, sidebarHeadProjections);

            var sidebarSideProjections = moduleDescriptor.RegisterSidebarSideProjections(containerBuilder);
            UpdateConcurrentBagWithEnumerator(knownSidebarContentItems, sidebarSideProjections);

            var sidebarBodyProjections = moduleDescriptor.RegisterSidebarMainProjections(containerBuilder);
            UpdateConcurrentBagWithEnumerator(knownSidebarBodyContentItems, sidebarBodyProjections);

            var siteSettingsProjections = moduleDescriptor.RegisterSiteSettingsProjections(containerBuilder);
            UpdateConcurrentBagWithEnumerator(knownSiteSettingsItems, siteSettingsProjections);

            var styleSheetFiles = moduleDescriptor.RegisterCssIncludes();
            if (styleSheetFiles != null)
            {
                foreach (var styleSheetFile in styleSheetFiles)
                {
                    knownStyleSheetIncludes.Add(styleSheetFile);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterKnownModuleRoutes(RouteCollection routes)
        {
            foreach (var context in knownModules)
            {
                foreach (var moduleRoute in context.Value.Routes)
                {
                    routes.Add(moduleRoute);
                }
            }
        }

        /// <summary>
        /// Initializes all known modules.
        /// </summary>        
        public void InitializeModules()
        {
            if (knownModuleDescriptorTypes != null && knownModuleDescriptorTypes.Count > 0)
            {
                ContainerBuilder containerBuilder = new ContainerBuilder();
                foreach (var moduleDescriptorType in knownModuleDescriptorTypes.Values)
                {
                    containerBuilder.RegisterType(moduleDescriptorType).AsSelf().SingleInstance();
                }

                ContextScopeProvider.RegisterTypes(containerBuilder);

                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    var moduleDescriptors = new List<ModuleDescriptor>();
                    foreach (var moduleDescriptorType in knownModuleDescriptorTypes.Values)
                    {
                        if (container.IsRegistered(moduleDescriptorType))
                        {
                            var moduleDescriptor = container.Resolve(moduleDescriptorType) as ModuleDescriptor;
                            moduleDescriptors.Add(moduleDescriptor);
                        }
                        else
                        {
                            Log.WarnFormat("Failed to resolve module instance from type {0}.", moduleDescriptorType.FullName);
                        }
                    }

                    moduleDescriptors = moduleDescriptors.OrderBy(f => f.Order).ToList();
                    foreach (var moduleDescriptor in moduleDescriptors)
                    {
                        try
                        {
                            RegisterModule(moduleDescriptor);                            
                        }
                        catch (Exception ex)
                        {
                            Log.ErrorFormat("Failed to register module of type {0}.", ex, moduleDescriptor.GetType().FullName);
                        }
                    }
                }
            }
            else
            {
                Log.Info("No registered module descriptors found.");
            }
        }

        /// <summary>
        /// Determines whether type is module descriptor type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if type is module descriptor type; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsModuleDescriptorType(Type type)
        {
            return typeof(ModuleDescriptor).IsAssignableFrom(type) && type.IsPublic;
        }

        /// <summary>
        /// Updates the concurrent bag with enumerator.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="bag">The bag.</param>
        /// <param name="enumerator">The enumerator.</param>
        private static void UpdateConcurrentBagWithEnumerator<T>(List<T> bag, IEnumerable<T> enumerator)
        {            
            if (enumerator != null)
            {
                foreach (var item in enumerator)
                {
                    bag.Add(item);
                }
            }
        }
    }
}
