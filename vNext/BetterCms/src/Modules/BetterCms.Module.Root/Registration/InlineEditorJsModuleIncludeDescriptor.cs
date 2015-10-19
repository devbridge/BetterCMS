using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

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
        /// <param name="loggerFactory">The logger factory</param>
        public InlineEditorJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.inlineEdit")
        {

            Links = new IActionUrlProjection[]
                {
                };

            Globalization = new IActionProjection[]
                {      
                    new JavaScriptModuleGlobalization(this, "messageDeleting", () => RootGlobalization.Message_Deleting, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "messageSaving", () => RootGlobalization.Message_Saving, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "confirmDeleteMessage", () => RootGlobalization.Confirm_Delete_DefaultMessage, loggerFactory),
                };
        }
    }
}