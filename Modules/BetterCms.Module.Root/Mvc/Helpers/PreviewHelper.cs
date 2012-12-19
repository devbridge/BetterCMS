using System.Web;
using System.Text;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class PreviewHelper
    {
        public static IHtmlString PreviewContentBox(this HtmlHelper html, string url, string title = "Content Preview")
        {
            return PreviewBox(html, url, title, 372, 372, "bcms-content-frame");                
        }

        public static IHtmlString PreviewLayoutBox(this HtmlHelper html, string url, string title = "Layout Preview")
        {
            return PreviewBox(html, url, title, 930, 930, "bcms-layout-frame");            
        }

        private static IHtmlString PreviewBox(HtmlHelper html, string url, string title, int width, int height, string frameCssClass)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<div class=\"bcms-preview-box\">");
            sb.AppendFormat("<iframe class=\"{0}\" width=\"{1}\" height=\"{2}\" scrolling=\"no\" border=\"0\" frameborder=\"0\" src=\"{3}\" />", frameCssClass, width, height, html.Encode(url));
            sb.AppendLine();
            sb.AppendFormat("<div class=\"bcms-zoom-overlay\" data-preview-title=\"{0}\" data-preview-url=\"{1}\"> </div>", html.Encode(title), html.Encode(url));
            sb.AppendLine();
            sb.AppendLine("</div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}