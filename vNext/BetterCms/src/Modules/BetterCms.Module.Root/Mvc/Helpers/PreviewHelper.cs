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

            sb.AppendFormat(
                "<div class=\"bcms-preview-box\" data-as-image=\"{0}\", data-preview-url=\"{1}\", data-title=\"{2}\", data-frame-css-class=\"{3}\", data-width=\"{4}\", data-height=\"{5}\"><div class=\"bcms-preview-box-image\">",
                asImage,
                html.Encode(previewUrl),
                html.Encode(title),
                frameCssClass,
                width,
                height);
            sb.AppendLine("</div>");
            sb.AppendFormat("<div class=\"bcms-zoom-overlay\" data-preview-title=\"{0}\" data-preview-url=\"{1}\"> </div>", html.Encode(title), html.Encode(openUrl));
            sb.AppendLine();
            sb.AppendLine("</div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}