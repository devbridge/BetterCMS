using System;
using System.Linq;

using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.MediaManager.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Media Manager API Context.
    /// </summary>
    public partial class MediaManagerApiContext
    {
        /// <summary>
        /// Updates the folder content types.
        /// </summary>
        /// <exception cref="CmsApiException"></exception>
        internal void UpdateFolderContentTypes()
        {
            try
            {
                UnitOfWork.BeginTransaction();

                var medias = Repository
                    .AsQueryable<MediaFolder>()
                    .Where(m => m.ContentType != MediaContentType.Folder)
                    .ToList();

                foreach (var media in medias)
                {
                    media.ContentType = MediaContentType.Folder;
                    Repository.Save(media);
                }

                UnitOfWork.Commit();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to update medias content types.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}