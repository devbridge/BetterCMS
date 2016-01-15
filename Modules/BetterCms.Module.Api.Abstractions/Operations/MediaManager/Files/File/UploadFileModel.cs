// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadFileModel.cs" company="Devbridge Group LLC">
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
using System.IO;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.MediaManager.Files.File
{
    /// <summary>
    /// The upload file model.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UploadFileModel: SaveModelBase
    {
        /// <summary>
        /// Gets or sets file id.
        /// </summary>
        [DataMember]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets folder id.
        /// </summary>
        [DataMember]
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets file title.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets file description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets file name.
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets file stream.
        /// </summary>
        [DataMember]
        public Stream FileStream { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to wait for upload result or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if to wait for upload result; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool WaitForUploadResult { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public IList<AccessRuleModel> AccessRules { get; set; }
    }
}
