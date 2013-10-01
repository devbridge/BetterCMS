using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Folder
{
    /// <summary>
    /// A command to delete given folder.
    /// </summary>
    public class DeleteFolderCommand : CommandBase, ICommand<DeleteFolderCommandRequest, bool>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteFolderCommandRequest request)
        {
            UnitOfWork.BeginTransaction();
            var mediaFolder = Repository.Delete<MediaFolder>(request.FolderId, request.Version);
            DeleteSubMedias(mediaFolder);
            UnitOfWork.Commit();

            Events.MediaManagerEvents.Instance.OnMediaFolderDeleted(mediaFolder);

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
                var file = subItem as MediaFile;
                if (file != null)
                {
                    file.AccessRules.Clear();
                }

                Repository.Delete(subItem);
                DeleteSubMedias(subItem);
            }
        }
    }
}