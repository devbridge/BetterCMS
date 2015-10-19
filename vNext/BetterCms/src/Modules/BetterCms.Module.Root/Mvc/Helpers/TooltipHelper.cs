using System.Text;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class TooltipHelper
    {
        public static IHtmlContent Tooltip(this IHtmlHelper htmlHelper, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"bcms-tooltip-mark\"");
            sb.AppendFormat(" data-message=\"{0}\"", htmlHelper.Encode(message));
            sb.Append("></div>");
            return new HtmlString(sb.ToString());
        }
    }
}