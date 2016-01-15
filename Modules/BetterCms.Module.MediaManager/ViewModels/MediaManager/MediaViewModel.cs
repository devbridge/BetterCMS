// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaViewModel.cs" company="Devbridge Group LLC">
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
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.MediaManager.Models;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    public class MediaViewModel : IEditableGridItem, IAccessSecuredViewModel
    {
        public virtual Guid Id { get; set; }

        public virtual int Version { get; set; }
        
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public virtual string Name { get; set; }

        public virtual MediaType Type { get; set; }
        
        public virtual MediaContentType ContentType { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual Guid? ParentFolderId { get; set; }

        public virtual string ParentFolderName { get; set; }

        public virtual string Tooltip { get; set; }

        public virtual string ThumbnailUrl { get; set; }

        public virtual bool IsReadOnly { get; set; }

        public MediaViewModel()
        {
            ContentType = MediaContentType.File;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Name: {2}, Type: {3}, ContentType: {4}, IsArchived: {5}, ParentFolderId: {6}, ParentFolderName: {7}", Id, Version, Name, Type, ContentType, IsArchived, ParentFolderId, ParentFolderName);
        }
    }
}