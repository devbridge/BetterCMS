using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Web.Mvc.Commands;

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

            var deletedMedias = new List<Media> { mediaFolder };
            DeleteSubMedias(mediaFolder, deletedMedias);

            UnitOfWork.Commit();

            // Notify
            foreach (var deletedMedia in deletedMedias)
            {
                var deletedFolder = deletedMedia as MediaFolder;
                if (deletedFolder != null)
                {
                    Events.MediaManagerEvents.Instance.OnMediaFolderDeleted(deletedFolder);
                }
                else
                {
                    var deletedFile = deletedMedia as MediaFile;
                    if (deletedFile != null)
                    {
                        Events.MediaManagerEvents.Instance.OnMediaFileDeleted(deletedFile);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Deletes the sub medias.
        /// </summary>
        /// <param name="media">The parent media.</param>
        /// <param name="deletedMedias">The deleted medias.</param>
        private void DeleteSubMedias(IEntity media, List<Media> deletedMedias)
        {
            var subItems = Repository.AsQueryable<Media>().Where(m => !m.IsDeleted && m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                var file = subItem as MediaFile;
                if (file != null)
                {
                    file.AccessRules.Clear();
                }

                deletedMedias.Add(subItem);

                Repository.Delete(subItem);
                DeleteSubMedias(subItem, deletedMedias);
            }
        }
    }
}