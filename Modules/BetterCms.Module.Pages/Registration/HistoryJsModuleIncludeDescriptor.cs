// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
    /// bcms.pages.seo.js module descriptor.
    /// </summary>
    public class HistoryJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeoJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public HistoryJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.pages.history")
        {

            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadContentHistoryDialogUrl", controller => controller.ContentHistory("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadContentVersionPreviewUrl", controller => controller.ContentVersion("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "loadContentVersionPreviewPropertiesUrl", controller => controller.ContentVersionProperties("{0}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "restoreContentVersionUrl", controller => controller.RestorePageContentVersion("{0}", "{1}", "{3}")),
                    new JavaScriptModuleLinkTo<HistoryController>(this, "destroyContentDraftVersionUrl", controller => controller.DestroyContentDraft("{0}", "{1}", "{2}"))
                };

            Globalization = new IActionProjection[]
                {
                     new JavaScriptModuleGlobalization(this, "contentHistoryDialogTitle", () => PagesGlobalization.ContentHistory_DialogTitle),
                     new JavaScriptModuleGlobalization(this, "contentVersionRestoreConfirmation", () => PagesGlobalization.ContentHistory_Restore_ConfirmationMessage),
                     new JavaScriptModuleGlobalization(this, "contentVersionDestroyDraftConfirmation", () => PagesGlobalization.ContentHistory_DestroyDraft_ConfirmationMessage),
                     new JavaScriptModuleGlobalization(this, "restoreButtonTitle", () => PagesGlobalization.ContentHistory_Restore_AcceptButtonTitle),
                     new JavaScriptModuleGlobalization(this, "destroyButtonTitle", () => PagesGlobalization.ContentHistory_Destroy_AcceptButtonTitle),
                     new JavaScriptModuleGlobalization(this, "closeButtonTitle", () => RootGlobalization.Button_Close),
                     new JavaScriptModuleGlobalization(this, "versionPreviewNotAvailableMessage", () => PagesGlobalization.ContentHistory_VersionPreviewNotAvailableMessage),
                };
        }
    }
}