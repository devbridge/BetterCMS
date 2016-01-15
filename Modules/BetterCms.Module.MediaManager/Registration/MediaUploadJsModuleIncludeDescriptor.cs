// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaUploadJsModuleIncludeDescriptor.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Controllers;

namespace BetterCms.Module.MediaManager.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class MediaUploadJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public MediaUploadJsModuleIncludeDescriptor(CmsModuleDescriptor module)
            : base(module, "bcms.media.upload")
        {            
            Links = new IActionProjection[]
                {    
                    new JavaScriptModuleLinkTo<UploadController>(this, "loadUploadFilesDialogUrl", f => f.MultiFileUpload("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<UploadController>(this, "uploadFileToServerUrl", f => f.UploadMedia(null)),
                    new JavaScriptModuleLinkTo<UploadController>(this, "undoFileUploadUrl", f => f.RemoveFileUpload("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<UploadController>(this, "loadUploadSingleFileDialogUrl", f => f.SingleFileUpload("{0}", "{1}", "{2}")),
                    new JavaScriptModuleLinkTo<UploadController>(this, "checkUploadedFileStatuses", f => f.CheckFilesStatuses(null))
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "uploadFilesDialogTitle", () => MediaGlobalization.MultiFileUpload_DialogTitle),
                    new JavaScriptModuleGlobalization(this, "failedToProcessFile", () => MediaGlobalization.MediaManager_FailedToProcessFile_Message),
                    new JavaScriptModuleGlobalization(this, "multipleFilesWarningMessageOnReupload", () => MediaGlobalization.MediaManager_MultipleFilesWarning_Message)
                };
        }
    }
}