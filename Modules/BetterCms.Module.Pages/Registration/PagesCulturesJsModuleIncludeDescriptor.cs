using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.cultures.js module descriptor.
    /// </summary>
    public class PagesCulturesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagesCulturesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagesCulturesJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.pages.cultures")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "suggestUntranslatedPagesUrl", c => c.SuggestUntranslatedPages(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "unassignTranslationConfirmation", () => PagesGlobalization.EditPageTranslations_UnassignTranslation_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "invariantCulture", () => RootGlobalization.InvariantCulture_Title)
                };
        }
    }
}