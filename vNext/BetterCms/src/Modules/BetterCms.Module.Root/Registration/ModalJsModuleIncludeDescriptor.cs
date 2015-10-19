using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class ModalJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public ModalJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.modal")
        {

            Links = new IActionUrlProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "save", () => RootGlobalization.Button_Save, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "cancel", () => RootGlobalization.Button_Cancel, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "ok", () => RootGlobalization.Button_Ok, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "saveDraft", () => RootGlobalization.Button_SaveDraft, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "saveAndPublish", () => RootGlobalization.Button_SaveAndPublish, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "preview", () => RootGlobalization.Button_Preview, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "yes", () => RootGlobalization.Button_Yes, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "no", () => RootGlobalization.Button_No, loggerFactory)
                };
        }
    }
}