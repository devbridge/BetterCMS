using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;

namespace BetterCms.Module.MediaManager.Services
{
    public class DefaultMediaService : IMediaService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultMediaService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Notifies, when medias are deleted.
        /// </summary>
        /// <param name="medias">The array of medias.</param>
        public void NotifiyMediaDeleted(IEnumerable<Models.Media> medias)
        {
            foreach (var media in medias)
            {
                if (media is Models.MediaFolder)
                {
                    Events.MediaManagerEvents.Instance.OnMediaFolderDeleted((Models.MediaFolder)media);
                }
                else if (media is Models.MediaFile)
                {
                    Events.MediaManagerEvents.Instance.OnMediaFileDeleted((Models.MediaFile)media);
                }
            }
        }

        /// <summary>
        /// Deletes sub items of media
        /// </summary>
        /// <param name="media">The media entity.</param>
        public IList<Models.Media> DeleteSubMedias(Models.Media media)
        {
            var deletedMedias = new List<Models.Media>();

            // Delete folder dependencies
            var folder = media as Models.MediaFolder;

            if (folder != null)
            {
                var dependencies = repository.
                    AsQueryable<Models.MediaFolderDependency>(d => d.Parent == folder).
                    ToList();

                foreach (var dependecy in dependencies)
                {
                    repository.Delete(dependecy);
                }
            }

            // Delete sub-items (folder children)
            var subItems = repository.
                AsQueryable<Models.Media>().
                Where(m => !m.IsDeleted && m.Folder != null && m.Folder.Id == media.Id).ToList();

            foreach (var subItem in subItems)
            {
                repository.Delete(subItem);
                deletedMedias.Add(subItem);

                var deletedSubMedias = DeleteSubMedias(subItem);
                deletedMedias.AddRange(deletedSubMedias);
            }

            return deletedMedias;
        }
    }
}