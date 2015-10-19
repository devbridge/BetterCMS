using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Registration
{
    public class OptionsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public OptionsJsModuleIncludeDescriptor(RootModuleDescriptor module, ILoggerFactory loggerFactory)
            : base(module, "bcms.options")
        {

            Links = new IActionUrlProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "deleteOptionConfirmMessage", () => RootGlobalization.DeleteOption_Confirmation_Message, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "datePickerTooltipTitle", () => RootGlobalization.Date_Picker_Tooltip_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionValidationMessage", () => RootGlobalization.Option_Invalid_Message, loggerFactory),

                    new JavaScriptModuleGlobalization(this, "optionTypeText", () => RootGlobalization.OptionTypes_Text_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeMultilineText", () => RootGlobalization.OptionTypes_MultilineText_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeInteger", () => RootGlobalization.OptionTypes_Integer_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeBoolean", () => RootGlobalization.OptionTypes_Boolean_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeDateTime", () => RootGlobalization.OptionTypes_DateTime_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeFloat", () => RootGlobalization.OptionTypes_Float_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeCustom", () => RootGlobalization.OptionTypes_Custom_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeJavaScriptUrl", () => RootGlobalization.OptionTypes_JavaScriptUrl_Title, loggerFactory),
                    new JavaScriptModuleGlobalization(this, "optionTypeCssUrl", () => RootGlobalization.OptionTypes_CssUrl_Title, loggerFactory)
                };
        }
    }
}