using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

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
        public ContentTreeJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.content.tree")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {              
                    new JavaScriptModuleGlobalization(this, "contentsTreeTitle", () => RootGlobalization.ContentsTree_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "closeTreeButtonTitle", () => RootGlobalization.Button_Close),
                    new JavaScriptModuleGlobalization(this, "saveSortChanges", () => RootGlobalization.ContentsSort_SaveSortChanges_Button),
                    new JavaScriptModuleGlobalization(this, "resetSortChanges", () => RootGlobalization.ContentsSort_ResetSortChanges_Button),
                    new JavaScriptModuleGlobalization(this, "saveSortChangesConfirmation", () => RootGlobalization.ContentsSort_SaveSortChanges_ConfirmationMessage)
                };
        }
    }
}