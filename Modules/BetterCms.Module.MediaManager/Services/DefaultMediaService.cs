using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Services
{
    /// <summary>
    /// Default media image service.
    /// </summary>
    internal class DefaultMediaService : IMediaService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultMediaService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Deletes the media.
        /// </summary>
        /// <param name="media">The media.</param>
        public void DeleteMedia(Media media)
        {
            if (media.MediaTags != null)
            {
                foreach (var mediaTag in media.MediaTags)
                {
                    repository.Delete(mediaTag);
                }
            }

            if (media.Categories != null)
            {
                foreach (var category in media.Categories)
                {
                    repository.Delete(category);
                }
            }

            if (media is MediaFile)
            {
                MediaFile file = (MediaFile)media;
                if (file.AccessRules != null)
                {
                    var rules = file.AccessRules.ToList();
                    rules.ForEach(file.RemoveRule);
                }
            }

            repository.Delete(media);

            var subItems = repository.AsQueryable<Media>().Where(m => !m.IsDeleted && m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var item in subItems)
            {
                DeleteMedia(item);
            }
        }

        /// <summary>
        /// Archives the sub medias.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="archivedMedias">The archived medias.</param>
        public void ArchiveSubMedias(Media media, List<Media> archivedMedias)
        {
            var subItems = repository.AsQueryable<Media>().Where(m => m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                if (!subItem.IsArchived)
                {
                    subItem.IsArchived = true;
                    archivedMedias.Add(subItem);

                    repository.Save(subItem);
                }

                ArchiveSubMedias(subItem, archivedMedias);
            }
        }

        /// <summary>
        /// Unarchives the sub medias.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <param name="unarchivedMedias">The unarchived medias.</param>
        public void UnarchiveSubMedias(Media media, List<Media> unarchivedMedias)
        {
            var subItems = repository.AsQueryable<Media>().Where(m => m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                if (subItem.IsArchived)
                {
                    subItem.IsArchived = false;
                    unarchivedMedias.Add(subItem);

                    repository.Save(subItem);
                }

                UnarchiveSubMedias(subItem, unarchivedMedias);
            }
        }
    }
}