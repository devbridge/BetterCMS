using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Registration
{
    /// <summary>
    /// bcms.content.js module descriptor.
    /// </summary>
    public class ContentJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public ContentJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.content")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {              
                    new JavaScriptModuleGlobalization(this, "failedLoadDialogMessage", () => RootGlobalization.Message_FailedToLoadDialog),        
                    new JavaScriptModuleGlobalization(this, "forbiddenDialogMessage", () => RootGlobalization.Message_AccessForbidden),        
                };
        }
    }
}