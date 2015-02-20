using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Web.Mvc.Commands;

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

            var archivedMedias = new List<Media> { media };
            ArchiveSubMedias(media, archivedMedias);

            UnitOfWork.Commit();

            // Notify
            foreach (var archivedMedia in archivedMedias.Distinct())
            {
                Events.MediaManagerEvents.Instance.OnMediaArchived(archivedMedia);
            }

            return new MediaViewModel
            {
                Id = media.Id,
                Version = media.Version
            };
        }

        private void ArchiveSubMedias(IEntity media, List<Media> archivedMedias)
        {
            var subItems = Repository.AsQueryable<Media>().Where(m => m.Folder != null && m.Folder.Id == media.Id).ToList();
            foreach (var subItem in subItems)
            {
                if (!subItem.IsArchived)
                {
                    subItem.IsArchived = true;
                    archivedMedias.Add(subItem);

                    Repository.Save(subItem);
                }
                ArchiveSubMedias(subItem, archivedMedias);
            }
        }
    }
}