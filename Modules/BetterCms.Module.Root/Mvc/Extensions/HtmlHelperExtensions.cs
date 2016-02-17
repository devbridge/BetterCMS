// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString MultipleDropDown<TModel>(this HtmlHelper<TModel> htmlHelper, IEnumerable<SelectListItem> items, object htmlAttributes = null)
        {
            var select = new TagBuilder("select");
            var optionsBuilder = new StringBuilder();
            select.MergeAttributes(HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes));

            select.MergeAttribute("multiple", "multiple");

            foreach (var item in items)
            {
                var option = new TagBuilder("option");

                if (item.Selected)
                {
                    option.MergeAttribute("selected", "selected");
                }

                option.MergeAttribute("value", item.Value);
                option.SetInnerText(item.Text);

                optionsBuilder.AppendLine(option.ToString(TagRenderMode.Normal));
            }

            select.InnerHtml = optionsBuilder.ToString();

            return new MvcHtmlString(select.ToString(TagRenderMode.Normal));
        }
    }
}