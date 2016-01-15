// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionsJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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