// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetMediaTreeModel.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.MediaManager.MediaTree
{
    [DataContract]
    [Serializable]
    public class GetMediaTreeModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetMediaTreeModel" /> class.
        /// </summary>
        public GetMediaTreeModel()
        {
            IncludeImagesTree = true;
            IncludeFilesTree = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include images tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include images tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeImagesTree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the files of images tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include the files of images tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include files tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include files tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeFilesTree { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include the files of files tree.
        /// </summary>
        /// <value>
        ///   <c>true</c> if include the files of files tree; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias to response.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived items to response; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include access rules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include access rules; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeAccessRules { get; set; }
    }
}