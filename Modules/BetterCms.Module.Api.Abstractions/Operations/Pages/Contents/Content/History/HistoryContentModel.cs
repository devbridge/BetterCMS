// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoryContentModel.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [DataContract]
    [System.Serializable]
    public class HistoryContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [DataMember]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the original content id.
        /// </summary>
        /// <value>
        /// The original content id.
        /// </value>
        [DataMember]
        public System.Guid? OriginalContentId { get; set; }

        /// <summary>
        /// Gets or sets the date content published on.
        /// </summary>
        /// <value>
        /// The date content published on.
        /// </value>
        [DataMember]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the content publisher user name.
        /// </summary>
        /// <value>
        /// The content publisher user name.
        /// </value>
        [DataMember]
        public string PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date content archived on.
        /// </summary>
        /// <value>
        /// The date content archived on.
        /// </value>
        [DataMember]
        public System.DateTime? ArchivedOn { get; set; }

        /// <summary>
        /// Gets or sets the user name who archived the content.
        /// </summary>
        /// <value>
        /// The user name who archived the content.
        /// </value>
        [DataMember]
        public string ArchivedByUser { get; set; }

        /// <summary>
        /// Gets or sets the content history item status.
        /// </summary>
        /// <value>
        /// The content history item status.
        /// </value>
        [DataMember]
        public ContentStatus Status { get; set; }
    }
}