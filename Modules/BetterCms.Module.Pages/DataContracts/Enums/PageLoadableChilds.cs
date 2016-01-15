// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageLoadableChilds.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Pages.DataContracts.Enums
{
    /// <summary>
    /// Enumerator with flags, determining, which Page children referenced and collections must be loaded.
    /// </summary>
    [Flags]
    public enum PageLoadableChilds
    {
        /// <summary>
        /// No child references and collections.
        /// </summary>
        None = 0,

        /// <summary>
        /// The layout.
        /// </summary>
        Layout = 1 << 0,

        /// <summary>
        /// The layout region.
        /// </summary>
        LayoutRegion = 1 << 1,

        /// <summary>
        /// The tags.
        /// </summary>
        Tags = 1 << 2,

        /// <summary>
        /// The category.
        /// </summary>
        Category = 1 << 3,

        /// <summary>
        /// The image.
        /// </summary>
        Image = 1 << 4,

        /// <summary>
        /// The secondary image.
        /// </summary>
        SecondaryImage = 1 << 5,

        /// <summary>
        /// The featured image.
        /// </summary>
        FeaturedImage = 1 << 6,

        /// <summary>
        /// All specified child referenced and collections.
        /// </summary>
        All = LayoutRegion | Tags | Category | Image | SecondaryImage | FeaturedImage
    }
}