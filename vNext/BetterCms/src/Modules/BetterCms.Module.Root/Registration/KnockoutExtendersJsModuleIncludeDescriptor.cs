using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class KnockoutExtendersJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public KnockoutExtendersJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.ko.extenders")
        {
            Links = new IActionUrlProjection[]
                {   
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "maximumLengthMessage", () => RootGlobalization.Validation_MaximumLengthExceeded_Message, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "requiredFieldMessage", () => RootGlobalization.Validation_FieldIsRequired_Message, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "regularExpressionMessage", () => RootGlobalization.Validation_RegularExpression_Message, loggerFactory), 
                    new JavaScriptModuleGlobalization(this, "invalidEmailMessage", () => RootGlobalization.Validation_Email_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "invalidKeyMessage", () => RootGlobalization.Validation_PreventHtml_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "nonAlphanumericMessage", () => RootGlobalization.Validation_PreventNonAlphanumeric_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "activeDirectoryCompliantMessage", () => RootGlobalization.Validation_ActiveDirectoryCompliant_Message, loggerFactory)
                };
        }
    }
}