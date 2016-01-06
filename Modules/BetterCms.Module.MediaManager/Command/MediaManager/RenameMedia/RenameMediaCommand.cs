using System;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.MediaManager.RenameMedia
{
    public class RenameMediaCommand : CommandBase, ICommand<MediaViewModel, MediaViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        public MediaViewModel Execute(MediaViewModel request)
        {
            Media media = Repository.AsProxy<Media>(request.Id);

            UnitOfWork.BeginTransaction();
            Repository.Save(media.CreateHistoryItem());
            media.PublishedOn = DateTime.Now;

            media.Version = request.Version;
            media.Title = request.Name;

            Repository.Save(media);
            UnitOfWork.Commit();

            if (media is MediaFolder)
            {
                Events.MediaManagerEvents.Instance.OnMediaFolderUpdated((MediaFolder)media);
            }
            else if (media is MediaFile)
            {
                Events.MediaManagerEvents.Instance.OnMediaFileUpdated((MediaFile)media);
            }

            return new MediaViewModel
            {
                Id = media.Id,
                Version = media.Version,
                Name = media.Title
            };
        }
    }
}