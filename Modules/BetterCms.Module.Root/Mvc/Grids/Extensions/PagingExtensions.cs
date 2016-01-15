// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagingExtensions.cs" company="Devbridge Group LLC">
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
using System.Web;
using System.Web.Mvc;

using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.Root.Mvc.Grids.Extensions
{
    public static class PagingExtensions
    {
        /// <summary>
        /// Adds the paging to page.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TGridItem">The type of the grid item.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="model">The model.</param>
        /// <returns>Generated HTML string with paging</returns>
        public static IHtmlString RenderPaging<TModel, TGridItem>(this HtmlHelper<TModel> htmlHelper, SearchableGridViewModel<TGridItem> model)
            where TGridItem : IEditableGridItem
        {
            var pages = PagerHelper.RenderPager(model);
            return new MvcHtmlString(pages);
        }
    }
}