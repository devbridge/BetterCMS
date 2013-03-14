using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.widgets.js module descriptor.
    /// </summary>
    public class WidgetsJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetsJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public WidgetsJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.widgets")
        {

            Links = new IActionProjection[]
                        {
                            new JavaScriptModuleLinkTo<WidgetsController>(this, "loadSiteSettingsWidgetListUrl", c => c.Widgets(null)),
                            new JavaScriptModuleLinkTo<WidgetsController>(this, "loadCreateHtmlContentWidgetDialogUrl", controller => controller.CreateHtmlContentWidget()),
                            new JavaScriptModuleLinkTo<WidgetsController>(this, "loadEditHtmlContentWidgetDialogUrl", controller => controller.EditHtmlContentWidget("{0}"))
                            ,
                            new JavaScriptModuleLinkTo<WidgetsController>(this, "loadCreateServerControlWidgetDialogUrl", controller => controller.CreateServerControlWidget()),
                            new JavaScriptModuleLinkTo<WidgetsController>(this, "loadEditServerControlWidgetDialogUrl", controller => controller.EditServerControlWidget("{0}")),
                            new JavaScriptModuleLinkTo<WidgetsController>(this, "deleteWidgetUrl", controller => controller.DeleteWidget("{0}", "{1}")),
                            new JavaScriptModuleLinkTo<ContentController>(this, "loadPageContentOptionsDialogUrl", controller => controller.PageContentOptions("{0}"))
                        };

            Globalization = new IActionProjection[]
                                {
                                    new JavaScriptModuleGlobalization(this, "createHtmlContentWidgetDialogTitle", () => PagesGlobalization.CreateHtmlContentWidget_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "editAdvancedContentDialogTitle", () => PagesGlobalization.EditWidget_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "createWidgetDialogTitle", () => PagesGlobalization.CreateWidget_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "editWidgetDialogTitle", () => PagesGlobalization.EditWidget_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "editPageWidgetOptionsTitle", () => PagesGlobalization.PageWidgetOptions_Dialog_Title),
                                    new JavaScriptModuleGlobalization(this, "deleteWidgetConfirmMessage", () => PagesGlobalization.DeleteWidget_Confirmation_Message),
                                    new JavaScriptModuleGlobalization(this, "deleteOptionConfirmMessage", () => PagesGlobalization.DeleteOption_Confirmation_Message),
                                    new JavaScriptModuleGlobalization(this, "widgetStatusPublished", () => RootGlobalization.ContentStatus_Published),
                                    new JavaScriptModuleGlobalization(this, "widgetStatusDraft", () => RootGlobalization.ContentStatus_Draft),
                                    new JavaScriptModuleGlobalization(this, "widgetStatusPublishedWithDraft", () => RootGlobalization.ContentStatus_PublishedWithDraft),
                                    new JavaScriptModuleGlobalization(this, "previewImageNotFoundMessage", () => PagesGlobalization.EditWidget_PreviewImageNotFound_Message),
                                };
        }
    }
}