// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveLayoutModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [DataContract]
    [Serializable]
    public class SaveLayoutModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout path.
        /// </summary>
        /// <value>
        /// The layout path.
        /// </value>
        [DataMember]
        public string LayoutPath { get; set; }

        /// <summary>
        /// Gets or sets the preview URL.
        /// </summary>
        /// <value>
        /// The preview URL.
        /// </value>
        [DataMember]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets the list of layout options.
        /// </summary>
        /// <value>
        /// The list of layout options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<OptionModel> Options { get; set; }

        /// <summary>
        /// Gets or sets the list of layout regions.
        /// </summary>
        /// <value>
        /// The list of layout regions.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<RegionSaveModel> Regions { get; set; }
    }
}
