using System.Collections.Generic;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Module.Root.Mvc.Extensions
{
    internal static class HtmlAttributesUtility
    {
        // Methods
        public static IDictionary<string, object> ObjectToHtmlAttributesDictionary(object htmlAttributes)
        {
            IDictionary<string, object> dictionary = null;
            if (htmlAttributes == null)
            {
                return new Dictionary<string, object>();
            }
            dictionary = htmlAttributes as IDictionary<string, object> ??
                         HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes).FixHtmlAttributes();
            return dictionary;
        }

        public static Dictionary<string, object> FixHtmlAttributes(this IDictionary<string, object> htmlAttributes)
        {
            var keysToRemove = new List<string>();
            var newAttributes = new Dictionary<string, object>();
            foreach (var attr in htmlAttributes)
            {
                var converted = attr.Key.CamelToDash();

                keysToRemove.Add(attr.Key);
                newAttributes.Add(converted, attr.Value);
            }
            return newAttributes;
        }
    }
}