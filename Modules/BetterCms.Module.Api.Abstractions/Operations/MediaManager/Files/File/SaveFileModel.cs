// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveFileModel.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    [DataContract]
    [Serializable]
    public class SaveFileModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the media title.
        /// </summary>
        /// <value>
        /// The media title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public IList<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the public URL.
        /// </summary>
        /// <value>
        /// The public URL.
        /// </value>
        [DataMember]
        public string PublicUrl { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public IList<AccessRuleModel> AccessRules { get; set; }

        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        /// <value>
        /// The folder identifier.
        /// </value>
        [DataMember]
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the published on.
        /// </summary>
        /// <value>
        /// The published on.
        /// </value>
        [DataMember]
        public DateTime PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the original file.
        /// </summary>
        /// <value>
        /// The name of the original file.
        /// </value>
        [DataMember]
        public string OriginalFileName { get; set; }

        /// <summary>
        /// Gets or sets the original file extension.
        /// </summary>
        /// <value>
        /// The original file extension.
        /// </value>
        [DataMember]
        public string OriginalFileExtension { get; set; }

        /// <summary>
        /// Gets or sets the file URI.
        /// </summary>
        /// <value>
        /// The file URI.
        /// </value>
        [DataMember]
        public string FileUri { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataMember]
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is temporary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is temporary; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsTemporary { get; set; }

        /// <summary>
        /// Gets or sets the is uploaded.
        /// </summary>
        /// <value>
        /// The is uploaded.
        /// </value>
        [DataMember]
        public bool? IsUploaded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is canceled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is canceled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsCanceled { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail identifier.
        /// </summary>
        /// <value>
        /// The thumbnail identifier.
        /// </value>
        [DataMember]
        public Guid? ThumbnailId { get; set; }

        /// <summary>
        /// Gets or sets the Categories.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public IList<Guid> Categories { get; set; }
    }
}