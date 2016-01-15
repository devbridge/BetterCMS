// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultiFileUploadViewModel.cs" company="Devbridge Group LLC">
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
using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.MediaManager.ViewModels.Upload
{
    [Serializable]
    public class MultiFileUploadViewModel
    {
        public Guid RootFolderId { get; set; }

        public MediaType RootFolderType { get; set; }

        public Guid? SelectedFolderId { get; set; }

        public IList<Tuple<Guid, string>> Folders { get; set; }

        public IList<Guid> UploadedFiles { get; set; }

        public Guid ReuploadMediaId { get; set; }

        public bool ShouldOverride { get; set; }

        public List<UserAccessViewModel> UserAccessList { get; set; }

        public bool AccessControlEnabled { get; set; }

        public MultiFileUploadViewModel()
        {
            UserAccessList = new List<UserAccessViewModel>();
            ShouldOverride = true;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("RootFolderId: {0}, RootFolderType: {1}, SelectedFolderId: {2}, ReuploadMediaId: {3}", RootFolderId, RootFolderType, SelectedFolderId, ReuploadMediaId);
        }
    }
}