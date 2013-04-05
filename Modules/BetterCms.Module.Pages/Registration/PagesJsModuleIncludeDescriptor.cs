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
        public PagesJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.pages")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "loadAddNewPageDialogUrl", c => c.AddNewPage("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "loadSiteSettingsPageListUrl", c => c.Pages(null)),
                    new JavaScriptModuleLinkTo<PageController>(this, "deletePageConfirmationUrl", c => c.DeletePageConfirmation("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "changePublishStatusUrl", c => c.ChangePublishStatus(null)),
                    new JavaScriptModuleLinkTo<PageController>(this, "clonePageDialogUrl", c => c.ClonePage("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "convertStringToSlugUrl", c => c.ConvertStringToSlug("{0}", "{1}", "{2}"))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "addNewPageDialogTitle", () => PagesGlobalization.AddNewPage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "deletePageDialogTitle", () => PagesGlobalization.DeletePage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "pageDeletedMessage", () => PagesGlobalization.DeletePage_SuccessMessage_Message),
                    new JavaScriptModuleGlobalization(this, "pageDeletedTitle", () => PagesGlobalization.DeletePage_SuccessMessage_Title),
                    new JavaScriptModuleGlobalization(this, "clonePageDialogTitle", () => PagesGlobalization.ClonePage_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "cloneButtonTitle", () => PagesGlobalization.ClonePage_Dialog_CloneButton), 
                    new JavaScriptModuleGlobalization(this, "deleteButtonTitle", () => PagesGlobalization.DeletePage_Dialog_DeleteButton) 
                };
        }
    }
}