using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.widgets.js module descriptor.
    /// </summary>
    public class WidgetsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public WidgetsJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.widgets")
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
                            new JavaScriptModuleLinkTo<ContentController>(this, "loadPageContentOptionsDialogUrl", controller => controller.PageContentOptions("{0}")),
                            new JavaScriptModuleLinkTo<ContentController>(this, "loadChildContentOptionsDialogUrl", controller => controller.ChildContentOptions("{0}", "{1}", "{2}", "{3}")),
                            new JavaScriptModuleLinkTo<ContentController>(this, "getContentTypeUrl", controller => controller.GetContentType("{0}")),
                            new JavaScriptModuleLinkTo<WidgetsController>(this, "getWidgetUsagesUrl", controller => controller.WidgetUsages("{0}", null))
                        };

            Globalization = new IActionProjection[]
                        {
                            new JavaScriptModuleGlobalization(this, "createHtmlContentWidgetDialogTitle", () => PagesGlobalization.CreateHtmlContentWidget_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "editAdvancedContentDialogTitle", () => PagesGlobalization.EditWidget_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "createWidgetDialogTitle", () => PagesGlobalization.CreateWidget_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "editWidgetDialogTitle", () => PagesGlobalization.EditWidget_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "editPageWidgetOptionsTitle", () => PagesGlobalization.PageWidgetOptions_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "deleteWidgetConfirmMessage", () => PagesGlobalization.DeleteWidget_Confirmation_Message),
                            new JavaScriptModuleGlobalization(this, "widgetStatusPublished", () => RootGlobalization.ContentStatus_Published),
                            new JavaScriptModuleGlobalization(this, "widgetStatusDraft", () => RootGlobalization.ContentStatus_Draft),
                            new JavaScriptModuleGlobalization(this, "widgetStatusPublishedWithDraft", () => RootGlobalization.ContentStatus_PublishedWithDraft),
                            new JavaScriptModuleGlobalization(this, "previewImageNotFoundMessage", () => PagesGlobalization.EditWidget_PreviewImageNotFound_Message),
                            new JavaScriptModuleGlobalization(this, "deletingMessage", () => RootGlobalization.Message_Deleting),
                            new JavaScriptModuleGlobalization(this, "widgetUsageTitle", () => PagesGlobalization.SiteSettings_Widgets_PagesUsingWidget_Title),
                            new JavaScriptModuleGlobalization(this, "editChildWidgetOptionsTitle", () => PagesGlobalization.ChildWidgetOptions_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "editChildWidgetOptionsCloseButtonTitle", () => RootGlobalization.Button_Close),
                            
                            new JavaScriptModuleGlobalization(this, "widgetUsagesDialogTitle", () => PagesGlobalization.WidgetUsages_Dialog_Title),
                            new JavaScriptModuleGlobalization(this, "widgetUsagesType_Page", () => PagesGlobalization.WidgetUsages_Type_Page),
                            new JavaScriptModuleGlobalization(this, "widgetUsagesType_MasterPage", () => PagesGlobalization.WidgetUsages_Type_MasterPage),
                            new JavaScriptModuleGlobalization(this, "widgetUsagesType_HtmlWidget", () => PagesGlobalization.WidgetUsages_Type_HtmlWidget)
                        };
        }
    }
}