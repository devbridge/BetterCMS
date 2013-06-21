
using System.Collections.Generic;
using System.Text;

using BetterCms.Module.MediaManager.Content.Resources;

namespace BetterCms.Module.MediaManager.Helpers
{
    public static class MediaPreviewHelper
    {
        private static void AppendProperty(this StringBuilder html, string title, string value)
        {
            html.Append("<div class=\"bcms-file-info-text\">");
            html.Append("<b>");
            html.Append(title);
            html.Append(": ");
            html.Append("</b>");
            html.Append("<div class=\"bcms-editing-text\">");
            html.Append(value);
            html.Append("</div>");
            html.Append("</div>");
        }

        public static void RenderProperties(StringBuilder html, string title, IEnumerable<KeyValuePair<string, string>> properties)
        {
            html.Append("<div class=\"bcms-media-history-properties-holder\">");
                    html.Append("<div class=\"bcms-file-info-text\">");
                        html.Append("<div class=\"bcms-content-titles\">");
                            html.Append(MediaGlobalization.MediaHistory_Preview_PropertiesTitle);
                        html.Append("</div>");
                    html.Append("</div>");
                    foreach (var pair in properties)
                    {
                        html.AppendProperty(pair.Key, pair.Value);
                    }
            html.Append("</div>");
        }
    }
}