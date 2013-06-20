using System;
using System.Text;

namespace BetterCms.Module.MediaManager.Models.Extensions
{
    public static class MediaExtensions
    {
        public static Media CreateHistoryItem(this Media media)
        {
            var historyitem = media.Clone();
            historyitem.Original = media;
            return historyitem;
        }

        public static string GetPreviewHtml(this Media media)
        {
            var image = media as MediaImage;
            if (image != null)
            {
                return image.GetPreviewHtml();
            }

            var file = media as MediaFile;
            if (file != null)
            {
                return file.GetPreviewHtml();
            }

            return string.Empty;
        }

        private static string GetPreviewHtml(this MediaImage media)
        {
            var html = new StringBuilder();

            html.Append("<img src=\"");
            html.Append(media.PublicUrl);
            html.Append("\"/>");

            return html.ToString();
        }

        private static string GetPreviewHtml(this MediaFile media)
        {
            var html = new StringBuilder();

            html.Append("File preview - not implemented!"); // TODO:

            return html.ToString();
        }
    }
}