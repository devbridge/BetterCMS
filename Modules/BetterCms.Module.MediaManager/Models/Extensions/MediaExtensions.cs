using System;
using System.Text;

using BetterCms.Module.MediaManager.Content.Resources;

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
                return image.GetImagePreviewHtml();
            }

            var file = media as MediaFile;
            if (file != null)
            {
                return file.GetFilePreviewHtml();
            }

            return string.Empty;
        }
    }
}