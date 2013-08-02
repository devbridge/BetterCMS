using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.MediaManager.ArchiveMedia
{
    /// <summary>
    /// Command for archive a media
    /// </summary>
    public class ArchiveMediaCommand : CommandBase, ICommand<ArchiveMediaCommandRequest, MediaViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public MediaViewModel Execute(ArchiveMediaCommandRequest request)
        {
            UnitOfWork.BeginTransaction();

            Media media = Repository.AsProxy<Media>(request.Id);
            media.Version = request.Version;
            media.IsArchived = true;
            Repository.Save(media);
            ArchiveSubMedias(media);

            UnitOfWork.Commit();

            Events.MediaManagerEvents.Instance.OnMediaArchived(media);

            return new MediaViewModel
            {
                Id = media.Id,
                Version = media.Version
            };
        }

        private void ArchiveSubMedias(IEntity media)
        {
            var subItems = Repository.AsQueryable<Media>().Where(m => m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                if (!subItem.IsArchived)
                {
                    subItem.IsArchived = true;
                    Repository.Save(subItem);
                }
                ArchiveSubMedias(subItem);
            }
        }
    }
}