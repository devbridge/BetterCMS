// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageStatusFilterType.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Pages.ViewModels.Filter
{
    public enum PageStatusFilterType
    {
        /// <summary>
        /// Only published pages
        /// </summary>
        OnlyPublished = 1,

        /// <summary>
        /// Only unpublished pages
        /// </summary>
        OnlyUnpublished = 2,

        /// <summary>
        /// Pages, containing unpublished contents
        /// </summary>
        ContainingUnpublishedContents = 3
    }
}