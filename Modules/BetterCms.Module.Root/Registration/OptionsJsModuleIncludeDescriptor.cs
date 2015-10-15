using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Module.Root.Registration
{
    public class OptionsJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public OptionsJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.options")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {                    
                    new JavaScriptModuleGlobalization(this, "deleteOptionConfirmMessage", () => RootGlobalization.DeleteOption_Confirmation_Message),
                    new JavaScriptModuleGlobalization(this, "datePickerTooltipTitle", () => RootGlobalization.Date_Picker_Tooltip_Title),
                    new JavaScriptModuleGlobalization(this, "optionValidationMessage", () => RootGlobalization.Option_Invalid_Message),

                    new JavaScriptModuleGlobalization(this, "optionTypeText", () => RootGlobalization.OptionTypes_Text_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeMultilineText", () => RootGlobalization.OptionTypes_MultilineText_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeInteger", () => RootGlobalization.OptionTypes_Integer_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeBoolean", () => RootGlobalization.OptionTypes_Boolean_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeDateTime", () => RootGlobalization.OptionTypes_DateTime_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeFloat", () => RootGlobalization.OptionTypes_Float_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeCustom", () => RootGlobalization.OptionTypes_Custom_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeJavaScriptUrl", () => RootGlobalization.OptionTypes_JavaScriptUrl_Title),
                    new JavaScriptModuleGlobalization(this, "optionTypeCssUrl", () => RootGlobalization.OptionTypes_CssUrl_Title),

                    new JavaScriptModuleGlobalization(this, "invariantLanguage", () => RootGlobalization.InvariantLanguage_Title)
                };
        }
    }
}