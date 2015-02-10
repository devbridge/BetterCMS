using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.properties.js module descriptor.
    /// </summary>
    public class PagePropertiesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagePropertiesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagePropertiesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.properties")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "loadEditPropertiesDialogUrl", c => c.EditPageProperties("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "loadLayoutOptionsUrl", c => c.LoadLayoutOptions("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "loadLayoutUserAccessUrl", c => c.LoadLayoutUserAccess("{0}", "{1}")),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "editPagePropertiesModalTitle", () => PagesGlobalization.EditPageProperties_Title),
                    new JavaScriptModuleGlobalization(this, "editMasterPagePropertiesModalTitle", () => PagesGlobalization.EditPageProperties_EditMasterPage_Title),
                    new JavaScriptModuleGlobalization(this, "pageStatusChangeConfirmationMessagePublish", () => PagesGlobalization.EditPageProperties_PageStatusChange_ConfirmationMessage_Publish),
                    new JavaScriptModuleGlobalization(this, "pageStatusChangeConfirmationMessageUnPublish", () => PagesGlobalization.EditPageProperties_PageStatusChange_ConfirmationMessage_UnPublish),
                    new JavaScriptModuleGlobalization(this, "pageConversionToMasterConfirmationMessage", () => PagesGlobalization.EditPageProperties_PageConversionToMaster_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "selectedMasterIsChildPage", () => PagesGlobalization.SavePagePropertiesCommand_SelectedMasterIsChildPage_Message)
                };
        }
    }
}