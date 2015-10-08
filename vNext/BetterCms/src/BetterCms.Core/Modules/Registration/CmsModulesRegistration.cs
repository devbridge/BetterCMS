using System.Collections.Generic;

using Autofac;

using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Environment.Assemblies;
using BetterModules.Core.Models;
using BetterModules.Core.Modules.Registration;
using BetterModules.Core.Web.Modules.Registration;
using BetterModules.Core.Web.Mvc.Extensions;

namespace BetterCms.Core.Modules.Registration
{
    /// <summary>
    /// Default modules registration implementation.
    /// </summary>
    public class CmsModulesRegistration : DefaultWebModulesRegistration, ICmsModulesRegistration
    {
        /// <summary>
        /// Thread safe list of known java script modules dictionary.
        /// </summary>
        private readonly Dictionary<string, JsIncludeDescriptor> knownJavaScriptModules;

        /// <summary>
        /// Thread safe style sheet files collection.
        /// </summary>
        private readonly List<CssIncludeDescriptor> knownStyleSheetIncludes;

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
        /// The known CMS modules
        /// </summary>
        private readonly List<CmsModuleDescriptor> knownCmsModules;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsModulesRegistration" /> class.
        /// </summary>
        /// <param name="assemblyLoader">The assembly loader.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        public CmsModulesRegistration(IAssemblyLoader assemblyLoader, IControllerExtensions controllerExtensions)
            : base(assemblyLoader, controllerExtensions)
        {
            knownCmsModules = new List<CmsModuleDescriptor>();
            knownJavaScriptModules = new Dictionary<string, JsIncludeDescriptor>();
            knownStyleSheetIncludes = new List<CssIncludeDescriptor>();
            knownSidebarHeadContentItems = new List<IPageActionProjection>();
            knownSidebarContentItems = new List<IPageActionProjection>();
            knownSidebarBodyContentItems = new List<IPageActionProjection>();
            knownSiteSettingsItems = new List<IPageActionProjection>();
        }

        /// <summary>
        /// Gets the list of CMS modules.
        /// </summary>
        /// <returns>
        /// The lit of CMS modules
        /// </returns>
        public List<CmsModuleDescriptor> GetCmsModules()
        {
            return knownCmsModules;
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
        /// Gets known StyleSheet includes.
        /// </summary>
        /// <returns>Enumerator of known StyleSheet includes.</returns>
        public IEnumerable<CssIncludeDescriptor> GetStyleSheetIncludes()
        {
            return knownStyleSheetIncludes;
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
        /// Registers the types.
        /// </summary>
        /// <param name="registrationContext">The registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        protected override void RegisterModuleDescriptor(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder)
        {
            var descriptor = registrationContext.ModuleDescriptor as CmsModuleDescriptor;
            if (descriptor != null)
            {
                knownCmsModules.Add(descriptor);

                var jsModules = descriptor.RegisterJsIncludes();
                if (jsModules != null)
                {
                    foreach (var jsModuleDescriptor in jsModules)
                    {
                        knownJavaScriptModules.Add(jsModuleDescriptor.Name, jsModuleDescriptor);
                    }
                }

                var styleSheetFiles = descriptor.RegisterCssIncludes();
                if (styleSheetFiles != null)
                {
                    foreach (var styleSheetFile in styleSheetFiles)
                    {
                        knownStyleSheetIncludes.Add(styleSheetFile);
                    }
                }

                var sidebarHeadProjections = descriptor.RegisterSidebarHeaderProjections(containerBuilder);
                UpdateConcurrentBagWithEnumerator(knownSidebarHeadContentItems, sidebarHeadProjections);

                var sidebarSideProjections = descriptor.RegisterSidebarSideProjections(containerBuilder);
                UpdateConcurrentBagWithEnumerator(knownSidebarContentItems, sidebarSideProjections);

                var sidebarBodyProjections = descriptor.RegisterSidebarMainProjections(containerBuilder);
                UpdateConcurrentBagWithEnumerator(knownSidebarBodyContentItems, sidebarBodyProjections);

                var siteSettingsProjections = descriptor.RegisterSiteSettingsProjections(containerBuilder);
                UpdateConcurrentBagWithEnumerator(knownSiteSettingsItems, siteSettingsProjections);
            }

            base.RegisterModuleDescriptor(registrationContext, containerBuilder);
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
