using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root.Mvc;

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

            media.Version = request.Version;
            media.Title = request.Name;

            Repository.Save(media);
            UnitOfWork.Commit();

            return new MediaViewModel
            {
                Id = media.Id,
                Version = media.Version,
                Name = media.Title
            };
        }
    }
}