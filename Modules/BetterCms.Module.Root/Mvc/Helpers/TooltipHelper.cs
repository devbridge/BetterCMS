using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class TooltipHelper
    {
        public static IHtmlString Tooltip(this HtmlHelper htmlHelper, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"bcms-tooltip-mark\"");
            sb.AppendFormat(" data-message=\"{0}\"", HttpUtility.HtmlEncode(message));
            sb.Append("></div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}