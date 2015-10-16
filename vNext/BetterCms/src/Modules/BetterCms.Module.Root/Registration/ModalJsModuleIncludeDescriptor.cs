using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class ModalJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public ModalJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.modal")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "save", () => RootGlobalization.Button_Save), 
                    new JavaScriptModuleGlobalization(this, "cancel", () => RootGlobalization.Button_Cancel), 
                    new JavaScriptModuleGlobalization(this, "ok", () => RootGlobalization.Button_Ok),
                    new JavaScriptModuleGlobalization(this, "saveDraft", () => RootGlobalization.Button_SaveDraft),
                    new JavaScriptModuleGlobalization(this, "saveAndPublish", () => RootGlobalization.Button_SaveAndPublish),
                    new JavaScriptModuleGlobalization(this, "preview", () => RootGlobalization.Button_Preview),
                    new JavaScriptModuleGlobalization(this, "yes", () => RootGlobalization.Button_Yes),
                    new JavaScriptModuleGlobalization(this, "no", () => RootGlobalization.Button_No),
                };
        }
    }
}