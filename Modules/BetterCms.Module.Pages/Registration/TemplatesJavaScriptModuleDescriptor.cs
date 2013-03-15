using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    public class TemplatesJavaScriptModuleDescriptor: JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetsJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public TemplatesJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.template")
        {

            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadSiteSettingsTemplateListUrl", controller => controller.Templates(null)),
                           
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadRegisterTemplateDialogUrl", controller => controller.RegisterTemplate()),
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadEditTemplateDialogUrl", controller => controller.EditTemplate("{0}")),
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "deleteTemplateUrl", controller => controller.DeleteTemplate("{0}", "{1}")),
                            new JavaScriptModuleLinkTo<ContentController>(this, "loadTemplateRegionDialogUrl", controller => controller.PageContentOptions("{0}"))
                        };

            Globalization = new IActionProjection[]
                                {
                                    new JavaScriptModuleGlobalization(this, "createTemplateDialogTitle", () => PagesGlobalization.CreatTemplate_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "editTemplateDialogTitle", () => PagesGlobalization.EditTemplate_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "editTemplateRegionTitle", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "deleteTemplateConfirmMessage", () => PagesGlobalization.SiteSettings_Template_DeleteCategoryMessage),
                                    new JavaScriptModuleGlobalization(this, "deleteRegionConfirmMessage", () => PagesGlobalization.DeleteRegion_Confirmation_Message),
                                    new JavaScriptModuleGlobalization(this, "previewImageNotFoundMessage", () => PagesGlobalization.EditTemplate_PreviewImageNotFound_Message),
                                };
        }
    }
}