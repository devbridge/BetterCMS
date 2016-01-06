using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    public class MasterPagesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MasterPagesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public MasterPagesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.masterpage")
        {

            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadMasterPagesListUrl", controller => controller.MasterPages(null)),
                        };

            Globalization = new IActionProjection[]
                                {
                                    new JavaScriptModuleGlobalization(this, "masterPagesTabTitle", () => PagesGlobalization.SiteSettings_MasterPages_Title),
                                    new JavaScriptModuleGlobalization(this, "editMasterPagePropertiesModalTitle", () => PagesGlobalization.EditPageProperties_EditMasterPage_Title)
                                };
        }
    }
}