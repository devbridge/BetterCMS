using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.MediaManager.DeleteMedia
{
    /// <summary>
    /// Command for delete a media
    /// </summary>
    public class DeleteMediaCommand : CommandBase, ICommand<DeleteMediaCommandRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeleteMediaCommandRequest request)
        {
            UnitOfWork.BeginTransaction();
            var media = Repository.Delete<Media>(request.Id, request.Version, false);
            DeleteMedias(media);
            UnitOfWork.Commit();

            // Notify.
            if (media is MediaFolder)
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderDeleted((MediaFolder)media);
            }
            else if (media is MediaFile)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileDeleted((MediaFile)media);
            }        

            return true;
        }

        /// <summary>
        /// Deletes medias.
        /// </summary>
        /// <param name="media">The parent media.</param>
        private void DeleteMedias(Media media)
        {
            if (media.MediaTags != null)
            {
                foreach (var mediaTag in media.MediaTags)
                {
                    Repository.Delete(mediaTag);
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

            Repository.Delete(media);

            var subItems = Repository.AsQueryable<Media>().Where(m => !m.IsDeleted && m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var item in subItems)
            {                
                DeleteMedias(item);
            }
        }
    }
}