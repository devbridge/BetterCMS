using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.UI
{
    public static class TooltipHelper
    {
        public static IHtmlString Tooltip(this HtmlHelper htmlHelper, string title, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"bcms-tooltip-mark\"");
            sb.AppendFormat(" data-title=\"{0}\"", title);
            sb.AppendFormat(" data-message=\"{0}\"", HttpUtility.HtmlEncode(message));
            sb.Append("></div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}