using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.pages.inlineEdit.js module descriptor.
    /// </summary>
    public class InlineEditorJavaScriptModuleDescriptor : JavaScriptModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InlineEditorJavaScriptModuleDescriptor" /> class.
        /// </summary>
        /// <param name="containerModule">The container module.</param>
        public InlineEditorJavaScriptModuleDescriptor(ModuleDescriptor containerModule)
            : base(containerModule, "bcms.inlineEdit")
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