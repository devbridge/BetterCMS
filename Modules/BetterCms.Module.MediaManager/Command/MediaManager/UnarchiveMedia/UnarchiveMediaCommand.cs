using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.MediaManager.UnarchiveMedia
{
    /// <summary>
    /// Command for unarchive a media
    /// </summary>
    public class UnarchiveMediaCommand : CommandBase, ICommand<UnarchiveMediaCommandRequest, MediaViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public MediaViewModel Execute(UnarchiveMediaCommandRequest request)
        {
            UnitOfWork.BeginTransaction();

            Media media = Repository.AsProxy<Media>(request.Id);
            media.Version = request.Version;
            media.IsArchived = false;
            Repository.Save(media);
            UnarchiveSubMedias(media);

            UnitOfWork.Commit();

            Events.MediaManagerEvents.Instance.OnMediaUnarchived(media);

            return new MediaViewModel
            {
                Id = media.Id,
                Version = media.Version
            };
        }

        private void UnarchiveSubMedias(IEntity media)
        {
            var subItems = Repository.AsQueryable<Media>().Where(m => m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                if (subItem.IsArchived)
                {
                    subItem.IsArchived = false;
                    Repository.Save(subItem);
                }
                UnarchiveSubMedias(subItem);
            }
        }
    }
}