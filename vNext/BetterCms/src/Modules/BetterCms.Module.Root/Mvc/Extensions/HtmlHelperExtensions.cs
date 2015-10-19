using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Module.Root.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString MultipleDropDown<TModel>(this IHtmlHelper<TModel> htmlHelper, IEnumerable<SelectListItem> items, object htmlAttributes = null)
        {
            var select = new TagBuilder("select")
            {
                TagRenderMode = TagRenderMode.Normal
            };
            StringBuilder optionsBuilder = new StringBuilder();            
            select.MergeAttributes(HtmlAttributesUtility.ObjectToHtmlAttributesDictionary(htmlAttributes));

            select.MergeAttribute("multiple", "multiple");

            foreach (var item in items)
            {
                var option = new TagBuilder("option")
                {
                    TagRenderMode = TagRenderMode.Normal
                };

                if (item.Selected)
                {
                    option.MergeAttribute("selected", "selected");                    
                }

                option.MergeAttribute("value", item.Value);
                option.InnerHtml.Append(item.Text);

                optionsBuilder.AppendLine(option.ToString());
            }

            select.InnerHtml.Append(optionsBuilder.ToString());

            return new HtmlString(select.ToString());
        }
    }
}