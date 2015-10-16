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
            StringBuilder optionsBuilder = new StringBuilder();            
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