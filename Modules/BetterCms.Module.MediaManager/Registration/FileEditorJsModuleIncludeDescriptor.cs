// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileEditorJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.MediaManager.Controllers;
using BetterCms.Module.MediaManager.Content.Resources;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class FileEditorJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileEditorJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public FileEditorJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.media.fileeditor")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<FilesController>(this, "fileEditorDialogUrl", c => c.FileEditor("{0}")),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "fileEditorDialogTitle", () => MediaGlobalization.FileEditor_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "fileEditorUpdateFailureMessageTitle", () => MediaGlobalization.FileEditor_UpdateFailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "fileEditorUpdateFailureMessageMessage", () => MediaGlobalization.FileEditor_UpdateFailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "fileEditorHasChangesMessage", () => MediaGlobalization.FileEditor_HasChanges_Message)
                };
        }
    }
}