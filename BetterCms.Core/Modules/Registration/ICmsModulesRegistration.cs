using System.Collections.Generic;

using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Modules.Registration;

namespace BetterCms.Core.Modules.Registration
{
    /// <summary>
    /// Defines the contract for modules registration logic.
    /// </summary>
    public interface ICmsModulesRegistration : IWebModulesRegistration
    {
        /// <summary>
        /// Gets the list of CMS modules.
        /// </summary>
        /// <returns>The lit of CMS modules</returns>
        List<CmsModuleDescriptor> GetCmsModules();

        /// <summary>
        /// Gets all known JS modules.
        /// </summary>
        /// <returns>Enumerator of known JS modules.</returns>
        IEnumerable<JsIncludeDescriptor> GetJavaScriptModules();

        /// <summary>
        /// Gets the style sheet files.
        /// </summary>
        /// <returns>Enumerator of known modules style sheet files.</returns>
        IEnumerable<CssIncludeDescriptor> GetStyleSheetIncludes();

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
    }
}
