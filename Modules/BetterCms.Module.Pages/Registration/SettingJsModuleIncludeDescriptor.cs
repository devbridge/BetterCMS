using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.setting.js module descriptor.
    /// </summary>
    public class SettingJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public SettingJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.pages.setting")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SettingController>(this, "loadConfigurationSettingsListUrl", c => c.Settings(null))
                };

            Globalization = new IActionProjection[]
                {
                    //new JavaScriptModuleGlobalization(this, "sitemapCreatorDialogTitle", () => NavigationGlobalization.Sitemap_CreatorDialog_Title)
                };
        }
    }
}