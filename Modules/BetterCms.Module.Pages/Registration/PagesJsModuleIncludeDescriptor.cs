using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class PagesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "loadAddNewPageDialogUrl", c => c.AddNewPage("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "loadSiteSettingsPageListUrl", c => c.Pages(null)),
                    new JavaScriptModuleLinkTo<PageController>(this, "loadSelectPageUrl", c => c.SelectPage(null)),
                    new JavaScriptModuleLinkTo<PageController>(this, "deletePageConfirmationUrl", c => c.DeletePageConfirmation("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "changePublishStatusUrl", c => c.ChangePublishStatus(null)),
                    new JavaScriptModuleLinkTo<PageController>(this, "clonePageDialogUrl", c => c.ClonePage("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "clonePageWithLanguageDialogUrl", c => c.ClonePageWithLanguage("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "convertStringToSlugUrl", c => c.ConvertStringToSlug("{0}", "{1}", "{2}", "{3}", "{4}", "{5}"))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "addNewPageDialogTitle", () => PagesGlobalization.AddNewPage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "addNewMasterPageDialogTitle", () => PagesGlobalization.AddNewMasterPage_PageTitle),
                    new JavaScriptModuleGlobalization(this, "deletePageDialogTitle", () => PagesGlobalization.DeletePage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "pageDeletedMessage", () => PagesGlobalization.DeletePage_SuccessMessage_Message),
                    new JavaScriptModuleGlobalization(this, "pageDeletedTitle", () => PagesGlobalization.DeletePage_SuccessMessage_Title),
                    new JavaScriptModuleGlobalization(this, "clonePageDialogTitle", () => PagesGlobalization.ClonePage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "clonePageWithLanguageDialogTitle", () => PagesGlobalization.ClonePageWithLanguage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "cloneButtonTitle", () => PagesGlobalization.ClonePage_Dialog_CloneButton), 
                    new JavaScriptModuleGlobalization(this, "deleteButtonTitle", () => PagesGlobalization.DeletePage_Dialog_DeleteButton),
                    new JavaScriptModuleGlobalization(this, "pageStatusChangeConfirmationMessagePublish", () => PagesGlobalization.EditPageProperties_PageStatusChange_ConfirmationMessage_Publish),
                    new JavaScriptModuleGlobalization(this, "pageStatusChangeConfirmationMessageUnPublish", () => PagesGlobalization.EditPageProperties_PageStatusChange_ConfirmationMessage_UnPublish),
                    new JavaScriptModuleGlobalization(this, "selectPageDialogTitle", () => PagesGlobalization.SelectPage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "selectPageSelectButtonTitle", () => PagesGlobalization.SelectPage_Select_ButtonTitle),
                    new JavaScriptModuleGlobalization(this, "pageNotSelectedMessage", () => PagesGlobalization.SelectPage_PageIsNotSelected_Message),
                    new JavaScriptModuleGlobalization(this, "pagesListTitle", () => PagesGlobalization.Pages_List_Title),
                };
        }
    }
}