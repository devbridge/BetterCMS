// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagePropertiesJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.Registration
{
    /// <summary>
    /// bcms.pages.properties.js module descriptor.
    /// </summary>
    public class PagePropertiesJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagePropertiesJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public PagePropertiesJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.properties")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<PageController>(this, "loadEditPropertiesDialogUrl", c => c.EditPageProperties("{0}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "loadLayoutOptionsUrl", c => c.LoadLayoutOptions("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "loadLayoutUserAccessUrl", c => c.LoadLayoutUserAccess("{0}", "{1}")),
                    new JavaScriptModuleLinkTo<PageController>(this, "checkForMissingContentUrl", c => c.CheckForMissingContent("{0}", "{1}", "{2}"))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "editPagePropertiesModalTitle", () => PagesGlobalization.EditPageProperties_Title),
                    new JavaScriptModuleGlobalization(this, "editMasterPagePropertiesModalTitle", () => PagesGlobalization.EditPageProperties_EditMasterPage_Title),
                    new JavaScriptModuleGlobalization(this, "pageStatusChangeConfirmationMessagePublish", () => PagesGlobalization.EditPageProperties_PageStatusChange_ConfirmationMessage_Publish),
                    new JavaScriptModuleGlobalization(this, "pageStatusChangeConfirmationMessageUnPublish", () => PagesGlobalization.EditPageProperties_PageStatusChange_ConfirmationMessage_UnPublish),
                    new JavaScriptModuleGlobalization(this, "pageConversionToMasterConfirmationMessage", () => PagesGlobalization.EditPageProperties_PageConversionToMaster_ConfirmationMessage),
                    new JavaScriptModuleGlobalization(this, "selectedMasterIsChildPage", () => PagesGlobalization.SavePagePropertiesCommand_SelectedMasterIsChildPage_Message),
                    new JavaScriptModuleGlobalization(this, "missingContentConfirmationMessage", () => PagesGlobalization.EditPageProperties_ChangedLayoutMissingContent_Confirmation_Message)
                };
        }
    }
}