// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageEditorJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class ImageEditorJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEditorJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public ImageEditorJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.media.imageeditor")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<ImagesController>(this, "imageEditorDialogUrl", c => c.ImageEditor("{0}")),
                    new JavaScriptModuleLinkTo<ImagesController>(this, "imageEditorInsertDialogUrl", c => c.ImageEditorInsert("{0}"))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "imageEditorDialogTitle", () => MediaGlobalization.ImageEditor_Dialog_Title),
                    new JavaScriptModuleGlobalization(this, "imageEditorInsertDialogTitle", () => MediaGlobalization.ImageEditor_InsertDialog_Title),
                    new JavaScriptModuleGlobalization(this, "imageEditorInsertDialogAcceptButton", () => MediaGlobalization.ImageEditor_InsertDialog_AcceptButton),
                    new JavaScriptModuleGlobalization(this, "imageEditorUpdateFailureMessageTitle", () => MediaGlobalization.ImageEditor_UpdateFailureMessage_Title),
                    new JavaScriptModuleGlobalization(this, "imageEditorUpdateFailureMessageMessage", () => MediaGlobalization.ImageEditor_UpdateFailureMessage_Message),
                    new JavaScriptModuleGlobalization(this, "imageEditorHasChangesMessage", () => MediaGlobalization.ImageEditor_HasChanges_Message),
                    new JavaScriptModuleGlobalization(this, "saveButtonTitle", () => MediaGlobalization.ImageEditor_Save_Title),
                    new JavaScriptModuleGlobalization(this, "saveAsNewVersionButtonTitle", () => MediaGlobalization.ImageEditor_SaveAsNewVersion_Title),
                    new JavaScriptModuleGlobalization(this, "saveWithOverrideButtonTitle", () => MediaGlobalization.ImageEditor_SaveWithOverride_Title),
                    new JavaScriptModuleGlobalization(this, "closeButtonTitle", () => RootGlobalization.Button_Close)
                };
        }
    }
}