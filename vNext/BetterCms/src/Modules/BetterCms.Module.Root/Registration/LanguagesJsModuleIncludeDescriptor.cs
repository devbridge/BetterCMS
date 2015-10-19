using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

using BetterModules.Core.Web.Modules;
using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.Framework.Logging;

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
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="controllerExtensions">Controller extensions</param>
        public LanguagesJsModuleIncludeDescriptor(CmsModuleDescriptor module, ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions)
            : base(module, "bcms.languages")
        {

            Links = new IActionUrlProjection[]
                {
                    new JavaScriptModuleLinkTo<LanguageController>(this, "loadSiteSettingsLanguagesUrl", c => c.ListTemplate(), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "loadLanguagesUrl", c => c.LanguagesList(null), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "saveLanguageUrl", c => c.SaveLanguage(null), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "deleteLanguageUrl", c => c.DeleteLanguage(null, null), loggerFactory, controllerExtensions),
                    new JavaScriptModuleLinkTo<LanguageController>(this, "languageSuggestionUrl", c => c.SuggestLanguages(null), loggerFactory, controllerExtensions)
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "deleteLanguageConfirmMessage", () => RootGlobalization.DeleteLanguage_Confirmation_Message, loggerFactory)
                };
        }
    }
}