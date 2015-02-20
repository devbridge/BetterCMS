using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.languages.js module descriptor.
    /// </summary>
    public class LanguagesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguagesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public LanguagesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.languages")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<LanguageController>(this, "loadSiteSettingsLanguagesUrl", c => c.ListTemplate()),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "loadLanguagesUrl", c => c.LanguagesList(null)),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "saveLanguageUrl", c => c.SaveLanguage(null)),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "deleteLanguageUrl", c => c.DeleteLanguage(null, null)),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "languageSuggestionUrl", c => c.SuggestLanguages(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "deleteLanguageConfirmMessage", () => RootGlobalization.DeleteLanguage_Confirmation_Message)
                };
        }
    }
}