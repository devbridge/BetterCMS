// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateBoxHelper.cs" company="Devbridge Group LLC">
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
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BetterCms.Module.Root.Mvc.UI
{
    public static class DateBoxHelper
    {
        public static MvcHtmlString DateTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DateTime? value, IDictionary<string, object> htmlAttributes = null)
        {
            if (value.HasValue)
            {
                string date = value.Value.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
                htmlAttributes = htmlAttributes ?? new Dictionary<string, object>(1);
                htmlAttributes["Value"] = date;
            }
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }
    }
}