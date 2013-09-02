using System.Web;
using System.Text;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class PreviewHelper
    {
        public static IHtmlString PreviewContentBox(this HtmlHelper html, string previewUrl, string openUrl, string title = "Content Preview", bool asImage = false)
        {
            return PreviewBox(html, previewUrl, openUrl, title, "100%", "100%", "bcms-content-frame", asImage);                
        }

        public static IHtmlString PreviewLayoutBox(this HtmlHelper html, string url, string title = "Layout Preview")
        {
            return PreviewBox(html, url, url, title, "930", "930", "bcms-layout-frame", false);            
        }

        private static IHtmlString PreviewBox(HtmlHelper html, string previewUrl, string openUrl, string title, string width, string height, string frameCssClass, bool asImage)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<div class=\"bcms-preview-box\">");
            if (asImage)
            {
                sb.AppendFormat("<img src=\"{0}\" alt=\"{1}\" />", html.Encode(previewUrl), html.Encode(title));
            }
            else
            {
                sb.AppendFormat("<iframe class=\"{0}\" width=\"{1}\" height=\"{2}\" scrolling=\"no\" border=\"0\" frameborder=\"0\" src=\"{3}\" style=\"background-color:white;\"/>", frameCssClass, width, height, html.Encode(previewUrl));
            }
            sb.AppendLine();
            sb.AppendFormat("<div class=\"bcms-zoom-overlay\" data-preview-title=\"{0}\" data-preview-url=\"{1}\"> </div>", html.Encode(title), html.Encode(openUrl));
            sb.AppendLine();
            sb.AppendLine("</div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}