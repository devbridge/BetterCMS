using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.content.js module descriptor.
    /// </summary>
    public class PagesContentJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagesContentJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.content")
        {
            Links = new IActionProjection[]
                {      
                    new JavaScriptModuleLinkTo<ContentController>(this, "loadWidgetsUrl", controller => controller.Widgets("{0}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "loadAddNewHtmlContentDialogUrl", controller => controller.AddPageHtmlContent("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "insertContentToPageUrl", controller => controller.InsertContentToPage("{0}", "{1}", "{2}", "{3}", "{4}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "deletePageContentUrl", controller => controller.DeletePageContent("{0}", "{1}", "{2}", "{3}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "editPageContentUrl", controller => controller.EditPageHtmlContent("{0}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "sortPageContentUrl", controller => controller.SortPageContent(null)),
                    new JavaScriptModuleLinkTo<WidgetsController>(this, "selectWidgetUrl", controller => controller.SelectWidget(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "addNewContentDialogTitle", () => PagesGlobalization.AddNewContent_Dialog_Title),                                        
                    new JavaScriptModuleGlobalization(this, "editContentDialogTitle", () => PagesGlobalization.EditContent_Dialog_Title),                                        
                    
                    new JavaScriptModuleGlobalization(this, "insertingWidgetInfoMessage", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Message),
                    new JavaScriptModuleGlobalization(this, "insertingWidgetInfoHeader", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Header),
                    new JavaScriptModuleGlobalization(this, "insertingWidgetErrorMessage", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Message),
                    
                    new JavaScriptModuleGlobalization(this, "sortingPageContentMessage", () => PagesGlobalization.SortPageContent_Info_Message),

                    new JavaScriptModuleGlobalization(this, "deleteContentConfirmationTitle", () => PagesGlobalization.DeletePageContent_ConfirmationTitle),
                    new JavaScriptModuleGlobalization(this, "deleteContentConfirmationMessage", () => PagesGlobalization.DeletePageContent_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "deleteContentSuccessMessageTitle", () => PagesGlobalization.DeletePageContent_SuccessMessage_Title),
                    new JavaScriptModuleGlobalization(this, "deleteContentSuccessMessageMessage", () => PagesGlobalization.DeletePageContent_SuccessMessage_Message),
                    new JavaScriptModuleGlobalization(this, "deleteContentFailureMessageTitle", () => PagesGlobalization.DeletePageContent_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "deleteContentFailureMessageMessage", () => PagesGlobalization.DeletePageContent_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "sortPageContentFailureMessageTitle", () => PagesGlobalization.SortPageContent_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "sortPageContentFailureMessageMessage", () => PagesGlobalization.SortPageContent_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "datePickerTooltipTitle", () => RootGlobalization.Date_Picker_Tooltip_Title),
                                        
                    new JavaScriptModuleGlobalization(this, "errorTitle", () => RootGlobalization.Alert_ErrorTitle),
                    new JavaScriptModuleGlobalization(this, "selectWidgetDialogTitle", () => PagesGlobalization.Widgets_SelectWidget_DialogTitle)
                };
        }
    }
}