using System.Collections.Generic;
using System.Text;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Helpers;

namespace BetterCms.Module.MediaManager.Models.Extensions
{
    public static class MediaImageExtensions
    {
        internal static string GetImagePreviewHtml(this MediaImage media)
        {
            var html = new StringBuilder();

            // Wrapping div start.
            html.Append("<div class=\"bcms-media-history-holder\">");

            var properties = new List<KeyValuePair<string, string>>();
            properties.Add(MediaGlobalization.MediaHistory_Preview_Properties_Caption, media.Caption);
            properties.Add(MediaGlobalization.MediaHistory_Preview_Properties_Title, media.Title);
            properties.Add(MediaGlobalization.MediaHistory_Preview_Properties_Description, media.Description);
            properties.Add(MediaGlobalization.MediaHistory_Preview_Properties_FileSize, media.SizeAsText());
            properties.Add(MediaGlobalization.MediaHistory_Preview_Properties_ImageDimensions, string.Format("{0} x {1}", media.Width, media.Height));
            properties.Add(MediaGlobalization.MediaHistory_Preview_Properties_PublicUrl, string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", media.PublicUrl));
            properties.Add(MediaGlobalization.MediaHistory_Preview_Properties_PublicThumbnailUrl, string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", media.PublicThumbnailUrl));
            MediaPreviewHelper.RenderProperties(html, "Properties", properties);

            // Image
            html.Append("<div class=\"bcms-media-history-image-holder\">");
            html.Append(string.Format("<img src=\"{0}\"/>", media.PublicUrl));
            html.Append("</div>");

            // Wrapping div end.
            html.Append("</div>");

            return html.ToString();
        }
    }
}