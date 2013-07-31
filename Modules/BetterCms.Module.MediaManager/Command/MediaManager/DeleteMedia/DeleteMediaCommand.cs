using System.Linq;

using BetterCms.Core.DataContracts;
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
            DeleteSubMedias(media);
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
        /// Deletes the sub medias.
        /// </summary>
        /// <param name="media">The parent media.</param>
        private void DeleteSubMedias(IEntity media)
        {
            var subItems = Repository.AsQueryable<Media>().Where(m => !m.IsDeleted && m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                Repository.Delete(subItem);
                DeleteSubMedias(subItem);
            }
        }
    }
}