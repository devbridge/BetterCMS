using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.content.js module descriptor.
    /// </summary>
    public class PagesContentJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetsJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public PagesContentJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.pages.content", "/file/bcms-pages/scripts/bcms.pages.content")
        {

            Links = new IActionProjection[]
                {      
                    new JavaScriptModuleLinkTo<ContentController>(this, "loadWidgetsFromCategoryUrl", controller => controller.WidgetCategory("{1}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "loadWidgetsUrl", controller => controller.Widgets("{0}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "loadAddNewHtmlContentDialogUrl", controller => controller.AddPageContent("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "insertContentToPageUrl", controller => controller.InsertContentToPage("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "deletePageContentUrl", controller => controller.DeletePageContent("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "editPageContentUrl", controller => controller.EditPageContent("{0}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "sortPageContentUrl", controller => controller.SortPageContent(null)),
                   
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "addNewContentDialogTitle", () => PagesGlobalization.AddNewContent_Dialog_Title),                                        
                    new JavaScriptModuleGlobalization(this, "editContentDialogTitle", () => PagesGlobalization.EditContent_Dialog_Title),                                        
                    
                    new JavaScriptModuleGlobalization(this, "insertingWidgetInfoMessage", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Message),
                    new JavaScriptModuleGlobalization(this, "insertingWidgetInfoHeader", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Header),
                    new JavaScriptModuleGlobalization(this, "insertingWidgetErrorMessage", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Message),
                                        
                    new JavaScriptModuleGlobalization(this, "deleteContentConfirmationTitle", () => PagesGlobalization.DeletePageContent_ConfirmationTitle),
                    new JavaScriptModuleGlobalization(this, "deleteContentConfirmationMessage", () => PagesGlobalization.DeletePageContent_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "deleteContentSuccessMessageTitle", () => PagesGlobalization.DeletePageContent_SuccessMessage_Title),
                    new JavaScriptModuleGlobalization(this, "deleteContentSuccessMessageMessage", () => PagesGlobalization.DeletePageContent_SuccessMessage_Message),
                    new JavaScriptModuleGlobalization(this, "deleteContentFailureMessageTitle", () => PagesGlobalization.DeletePageContent_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "deleteContentFailureMessageMessage", () => PagesGlobalization.DeletePageContent_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "sortPageContentFailureMessageTitle", () => PagesGlobalization.SortPageContent_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "sortPageContentFailureMessageMessage", () => PagesGlobalization.SortPageContent_FailureMessage_Message),
                                        
                    new JavaScriptModuleGlobalization(this, "errorTitle", () => RootGlobalization.Alert_ErrorTitle),

                    new JavaScriptModuleGlobalization(this, "saveDraft", () => RootGlobalization.Button_SaveDraft),
                    new JavaScriptModuleGlobalization(this, "saveAndPublish", () => RootGlobalization.Button_SaveAndPublish),
                    new JavaScriptModuleGlobalization(this, "preview", () => RootGlobalization.Button_Preview),                                       
                };
        }
    }
}