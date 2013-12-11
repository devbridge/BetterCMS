using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.cultures.js module descriptor.
    /// </summary>
    public class CulturesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CulturesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public CulturesJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.cultures")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<CultureController>(this, "loadSiteSettingsCulturesUrl", c => c.ListTemplate()),
                    new JavaScriptModuleLinkTo<CultureController>(this, "loadCulturesUrl", c => c.CulturesList(null)),
                    new JavaScriptModuleLinkTo<CultureController>(this, "saveCultureUrl", c => c.SaveCulture(null)),
                    new JavaScriptModuleLinkTo<CultureController>(this, "deleteCultureUrl", c => c.DeleteCulture(null, null)),
                    new JavaScriptModuleLinkTo<CultureController>(this, "cultureSuggestionServiceUrl", c => c.SuggestCultures(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "deleteCultureConfirmMessage", () => RootGlobalization.DeleteCulture_Confirmation_Message)
                };
        }
    }
}