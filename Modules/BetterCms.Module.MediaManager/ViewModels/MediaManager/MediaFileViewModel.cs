// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaFileViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaFileViewModel : MediaViewModel, IAccessSecuredViewModel
    {
        public virtual long Size { get; set; }
        
        public virtual string SizeText { get; set; }

        public virtual string FileExtension { get; set; }

        public virtual string PublicUrl { get; set; }

        public virtual bool IsProcessing { get; set; }
        
        public virtual bool IsFailed { get; set; }

        public MediaFileViewModel()
        {
            Type = MediaType.File;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Size: {1}, FileExtension: {2}, PublicUrl: {3}, IsProcessing: {4}, IsFailed: {5}", base.ToString(), Size, FileExtension, PublicUrl, IsProcessing, IsFailed);
        }
    }
}