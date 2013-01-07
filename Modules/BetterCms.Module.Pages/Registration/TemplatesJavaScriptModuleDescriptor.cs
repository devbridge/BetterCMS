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
            : base(containerModule, "bcms.pages.template", "/file/bcms-pages/scripts/bcms.pages.template")
        {

            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadSiteSettingsTemplateListUrl", controller => controller.Templates(null)),
                           
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadRegisterTemplateDialogUrl", controller => controller.RegisterTemplate()),
                            new JavaScriptModuleLinkTo<TemplatesController>(this, "loadEditServerControlWidgetDialogUrl", controller => controller.EditTemplate("{0}")),
                            //new JavaScriptModuleLinkTo<TemplatesController>(this, "deleteWidgetUrl", controller => controller.SaveTemplate("{0}"),
                            //new JavaScriptModuleLinkTo<TemplatesController>(this, "deleteWidgetUrl", controller => controller.DeleteWidget("{0}", "{1}"))
                            new JavaScriptModuleLinkTo<ContentController>(this, "loadPageContentOptionsDialogUrl", controller => controller.PageContentOptions("{0}"))
                        };

            Globalization = new IActionProjection[]
                                {
                                    new JavaScriptModuleGlobalization(this, "createHtmlContentWidgetDialogTitle", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "editAdvancedContentDialogTitle", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "createWidgetDialogTitle", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "editWidgetDialogTitle", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "editPageWidgetOptionsTitle", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "deleteWidgetConfirmMessage", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                    new JavaScriptModuleGlobalization(this, "deleteOptionConfirmMessage", () => PagesGlobalization.SiteSettings_TemplatesMenuItem),
                                };
        }
    }
}