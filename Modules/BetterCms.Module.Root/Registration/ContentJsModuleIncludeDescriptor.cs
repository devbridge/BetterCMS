// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
                    new JavaScriptModuleGlobalization(this, "showMasterPagesPath", () => RootGlobalization.MasterPagesPath_ShowPath_Button),        
                    new JavaScriptModuleGlobalization(this, "hideMasterPagesPath", () => RootGlobalization.MasterPagesPath_HidePath_Button),
                    new JavaScriptModuleGlobalization(this, "currentPage", () => RootGlobalization.MasterPagesPath_CurrentPage_Title),
                    new JavaScriptModuleGlobalization(this, "saveSortChanges", () => RootGlobalization.ContentsSort_SaveSortChanges_Button),
                    new JavaScriptModuleGlobalization(this, "resetSortChanges", () => RootGlobalization.ContentsSort_ResetSortChanges_Button),
                    new JavaScriptModuleGlobalization(this, "saveSortChangesConfirmation", () => RootGlobalization.ContentsSort_SaveSortChanges_ConfirmationMessage),
                };
        }
    }
}