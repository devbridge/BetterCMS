using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.pages.inlineEdit.js module descriptor.
    /// </summary>
    public class InlineEditorJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InlineEditorJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public InlineEditorJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.inlineEdit")
        {

            Links = new IActionProjection[]
                {
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "messageDeleting", () => RootGlobalization.Message_Deleting), 
                    new JavaScriptModuleGlobalization(this, "messageSaving", () => RootGlobalization.Message_Saving),
                    new JavaScriptModuleGlobalization(this, "confirmDeleteMessage", () => RootGlobalization.Confirm_Delete_DefaultMessage),
                };
        }
    }
}