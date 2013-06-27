using System;

using BetterCms.Api;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.ViewModels.File;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Files.SaveFile
{
    /// <summary>
    /// Command to save image properties.
    /// </summary>
    public class SaveFileDataCommand : CommandBase, ICommandIn<FileViewModel>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Execute(FileViewModel request)
        {
            var mediaFile = Repository.First<MediaFile>(request.Id.ToGuidOrDefault());

            UnitOfWork.BeginTransaction();
            Repository.Save(mediaFile.CreateHistoryItem());
            mediaFile.PublishedOn = DateTime.Now;

            mediaFile.Title = request.Title;
            mediaFile.Version = request.Version.ToIntOrDefault();

            Repository.Save(mediaFile);
            UnitOfWork.Commit();

            MediaManagerApiContext.Events.OnMediaFileUpdated(mediaFile);
        }
    }
}
