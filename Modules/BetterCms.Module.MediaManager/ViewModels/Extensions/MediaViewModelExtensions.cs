using System;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

namespace BetterCms.Module.MediaManager.ViewModels.Extensions
{
    public static class MediaViewModelExtensions
    {
        public static MediaViewModel Fill(this MediaViewModel model, Media media)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (media == null)
            {
                throw new ArgumentNullException("media");
            }

            model.Id = media.Id;
            model.Version = media.Version;
            model.Name = media.Title;
            model.CreatedOn = media.CreatedOn;
            model.Type = media.Type;
            model.IsArchived = media.IsArchived;
            model.ParentFolderId = media.Folder != null ? media.Folder.Id : Guid.Empty;
            model.ParentFolderName = media.Folder != null ? media.Folder.Title : MediaGlobalization.MediaList_RootFolderName;
            model.Tooltip = media.Image != null ? media.Image.Caption : null;
            model.ThumbnailUrl = media.Image != null ? media.Image.PublicThumbnailUrl : null;

            return model;
        }
    }
}