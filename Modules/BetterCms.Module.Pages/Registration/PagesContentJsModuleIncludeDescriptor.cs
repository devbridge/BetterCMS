// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagesContentJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Controllers;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.content.js module descriptor.
    /// </summary>
    public class PagesContentJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WidgetsJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagesContentJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.content")
        {
            Links = new IActionProjection[]
                {      
                    new JavaScriptModuleLinkTo<ContentController>(this, "loadWidgetsUrl", controller => controller.Widgets("{0}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "loadAddNewHtmlContentDialogUrl", controller => controller.AddPageHtmlContent("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "insertContentToPageUrl", controller => controller.InsertContentToPage("{0}", "{1}", "{2}", "{3}", "{4}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "deletePageContentUrl", controller => controller.DeletePageContent("{0}", "{1}", "{2}", "{3}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "editPageContentUrl", controller => controller.EditPageHtmlContent("{0}")),
                    new JavaScriptModuleLinkTo<ContentController>(this, "sortPageContentUrl", controller => controller.SortPageContent(null)),
                    new JavaScriptModuleLinkTo<WidgetsController>(this, "selectWidgetUrl", controller => controller.SelectWidget(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "addNewContentDialogTitle", () => PagesGlobalization.AddNewContent_Dialog_Title),                                        
                    new JavaScriptModuleGlobalization(this, "editContentDialogTitle", () => PagesGlobalization.EditContent_Dialog_Title),                                        
                    
                    new JavaScriptModuleGlobalization(this, "insertingWidgetInfoMessage", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Message),
                    new JavaScriptModuleGlobalization(this, "insertingWidgetInfoHeader", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Header),
                    new JavaScriptModuleGlobalization(this, "insertingWidgetErrorMessage", () => PagesGlobalization.AddPageContent_InsertingWidget_Information_Message),
                    
                    new JavaScriptModuleGlobalization(this, "sortingPageContentMessage", () => PagesGlobalization.SortPageContent_Info_Message),

                    new JavaScriptModuleGlobalization(this, "deleteContentConfirmationTitle", () => PagesGlobalization.DeletePageContent_ConfirmationTitle),
                    new JavaScriptModuleGlobalization(this, "deleteContentConfirmationMessage", () => PagesGlobalization.DeletePageContent_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "deleteContentSuccessMessageTitle", () => PagesGlobalization.DeletePageContent_SuccessMessage_Title),
                    new JavaScriptModuleGlobalization(this, "deleteContentSuccessMessageMessage", () => PagesGlobalization.DeletePageContent_SuccessMessage_Message),
                    new JavaScriptModuleGlobalization(this, "deleteContentFailureMessageTitle", () => PagesGlobalization.DeletePageContent_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "deleteContentFailureMessageMessage", () => PagesGlobalization.DeletePageContent_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "sortPageContentFailureMessageTitle", () => PagesGlobalization.SortPageContent_FailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "sortPageContentFailureMessageMessage", () => PagesGlobalization.SortPageContent_FailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "datePickerTooltipTitle", () => RootGlobalization.Date_Picker_Tooltip_Title),
                                        
                    new JavaScriptModuleGlobalization(this, "errorTitle", () => RootGlobalization.Alert_ErrorTitle),
                    new JavaScriptModuleGlobalization(this, "selectWidgetDialogTitle", () => PagesGlobalization.Widgets_SelectWidget_DialogTitle)
                };
        }
    }
}