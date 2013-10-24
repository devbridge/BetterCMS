using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;

using BetterCms.Core.Modules.Projections;

namespace BetterCms.Core.Modules.Registration
{
    /// <summary>
    /// Defines the contract for modules registration logic.
    /// </summary>
    public interface IModulesRegistration
    {
        /// <summary>
        /// Tries to scan and register module type from assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        void AddModuleDescriptorTypeFromAssembly(Assembly assembly);
        
        /// <summary>
        /// Gets registered modules.
        /// </summary>
        /// <returns>Enumerator of registered modules.</returns>
        IEnumerable<ModuleRegistrationContext> GetModules();

        /// <summary>
        /// Gets all known JS modules.
        /// </summary>
        /// <returns>Enumerator of known JS modules.</returns>
        IEnumerable<JsIncludeDescriptor> GetJavaScriptModules();

        /// <summary>
        /// Gets action projections to render in the sidebar header.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar header.</returns>
        IEnumerable<IPageActionProjection> GetSidebarHeaderProjections();

        /// <summary>
        /// Gets action projections to render in the sidebar side.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar side.</returns>
        IEnumerable<IPageActionProjection> GetSidebarSideProjections();

        /// <summary>
        /// Gets action projections to render in the sidebar body container.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the sidebar body container.</returns>
        IEnumerable<IPageActionProjection> GetSidebarBodyProjections();

        /// <summary>
        /// Gets action projections to render in the site settings menu container.
        /// </summary>
        /// <returns>Enumerator of registered action projections to render in the site settings menu container.</returns>
        IEnumerable<IPageActionProjection> GetSiteSettingsProjections();

        /// <summary>
        /// Finds the module by area name.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <returns>Known module instance.</returns>
        ModuleDescriptor FindModuleByAreaName(string areaName);

        /// <summary>
        /// Determines whether module by area name is registered.
        /// </summary>
        /// <param name="areaName">Name of the area.</param>
        /// <returns>
        /// <c>true</c> if module by area name is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsModuleRegisteredByAreaName(string areaName);

        /// <summary>
        /// Starts known modules.
        /// </summary>        
        void InitializeModules();

        /// <summary>
        /// Gets the style sheet files.
        /// </summary>
        /// <returns>Enumerator of known modules style sheet files.</returns>
        IEnumerable<CssIncludeDescriptor> GetStyleSheetIncludes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        void RegisterKnownModuleRoutes(RouteCollection routes);
    }
}
