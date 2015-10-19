using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.content.tree.js module descriptor.
    /// </summary>
    public class ContentTreeJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTreeJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        /// <param name="loggerFactory">The logger factory</param>
        public ContentTreeJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.content.tree")
        {

            Links = new IActionUrlProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {              
                    new JavaScriptModuleGlobalization(this, "contentsTreeTitle", () => RootGlobalization.ContentsTree_Dialog_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "closeTreeButtonTitle", () => RootGlobalization.Button_Close, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "saveSortChanges", () => RootGlobalization.ContentsSort_SaveSortChanges_Button, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "resetSortChanges", () => RootGlobalization.ContentsSort_ResetSortChanges_Button, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "saveSortChangesConfirmation", () => RootGlobalization.ContentsSort_SaveSortChanges_ConfirmationMessage, loggerFactory)
                };
        }
    }
}