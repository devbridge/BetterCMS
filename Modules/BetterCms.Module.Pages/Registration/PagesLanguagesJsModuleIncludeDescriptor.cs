using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.languages.js module descriptor.
    /// </summary>
    public class PagesLanguagesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagesLanguagesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagesLanguagesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.languages")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "suggestUntranslatedPagesUrl", c => c.SuggestUntranslatedPages(null)),
                    new JavaScriptModuleLinkTo<PageController>(this, "searchUntranslatedPagesUrl", c => c.SearchUntranslatedPages(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "unassignTranslationConfirmation", () => PagesGlobalization.EditPageTranslations_UnassignTranslation_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "invariantLanguage", () => RootGlobalization.InvariantLanguage_Title),
                    new JavaScriptModuleGlobalization(this, "replaceItemWithCurrentLanguageConfirmation", () => PagesGlobalization.EditPageTranslations_ReplaceTranslationWithCurrentLanguage_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "replaceItemWithLanguageConfirmation", () => PagesGlobalization.EditPageTranslations_ReplaceTranslationWithLanguage_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "assigningPageHasSameCultureAsCurrentPageMessage", () => PagesGlobalization.EditPageTranslations_PageHasSameCulture_Message),
                };
        }
    }
}