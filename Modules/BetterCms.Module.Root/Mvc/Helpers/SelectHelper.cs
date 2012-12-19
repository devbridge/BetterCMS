using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Module.Root.Models;

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
    }
}