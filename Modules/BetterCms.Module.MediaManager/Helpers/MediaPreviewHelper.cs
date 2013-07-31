using System.Collections.Generic;
using System.Text;

namespace BetterCms.Module.MediaManager.Helpers
{
    public static class MediaPreviewHelper
    {
        private static void AppendProperty(this StringBuilder html, string title, string value)
        {
            if (value.Length > 500)
            {
                value = value.Substring(0, 497) + "...";
            }

            html.Append("<div class=\"bcms-file-info-text\">");
            html.Append("<b>");
            html.Append(title);
            html.Append(": ");
            html.Append("</b>");
            html.Append(value);
            html.Append("</div>");
        }

        public static void RenderProperties(StringBuilder html, IEnumerable<KeyValuePair<string, string>> properties)
        {
            html.Append("<div class=\"bcms-media-history-properties-holder\">");
            foreach (var pair in properties)
            {
                html.AppendProperty(pair.Key, pair.Value);
            }
            html.Append("</div>");
        }
    }
}