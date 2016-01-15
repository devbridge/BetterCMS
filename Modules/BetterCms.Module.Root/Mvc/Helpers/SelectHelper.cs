// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectHelper.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Module.Root.Models;

using FluentNHibernate.Utils;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class SelectHelper
    {
        public static SelectList ToSelectList<T>(this IEnumerable<T> source, string dataValueField, string dataTextField, object selectedValue)
        {
            return new SelectList(source, dataValueField, dataTextField, selectedValue ?? -1);
        }

        public static SelectList ToSelectList(this IEnumerable<LookupKeyValue> source, object selectedValue)
        {
            return new SelectList(source, "Key", "Value", selectedValue ?? -1);
        }

        public static IEnumerable<SelectListItem> ToSelectListItems(this IEnumerable<LookupKeyValue> source, IEnumerable<Guid> selectedValues)
        {
            if (source != null)
            {
                var values = selectedValues != null ? selectedValues.ToList() : new List<Guid>();

                foreach (var item in source)
                {
                    var isSelected = values.Any(s => string.CompareOrdinal(s.ToLowerInvariantString(), item.Key) == 0);

                    yield return new SelectListItem()
                    {
                        Selected = isSelected,
                        Value = item.Key,
                        Text = item.Value
                    };
                }
            }
        }
    }
}