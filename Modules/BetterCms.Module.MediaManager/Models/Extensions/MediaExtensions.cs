using System;
using System.Text;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.Root.Mvc;

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

        public static bool IsChild(this Media media, Guid currentFolderId, bool includeArchivedItems)
        {
            if (media == null)
            {
                return false;
            }

            if (media.IsDeleted || (media.Folder != null && media.Folder.IsDeleted))
            {
                return false;
            }

            if (!includeArchivedItems && (media.IsArchived || (media.Folder != null && media.Folder.IsArchived)))
            {
                return false;
            }

            if (currentFolderId.HasDefaultValue())
            {
                return true;
            }

            if (media.Folder != null && !media.Folder.IsDeleted && media.Folder.Id == currentFolderId)
            {
                return true;
            }

            return IsChild(media.Folder, currentFolderId, includeArchivedItems);
        }
    }
}