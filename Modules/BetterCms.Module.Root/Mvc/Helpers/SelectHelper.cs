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