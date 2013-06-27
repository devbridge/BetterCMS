using System;
using System.Globalization;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.File;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.MediaManager.Command.Files.GetFile
{
    /// <summary>
    /// Command to get media image data.
    /// </summary>
    public class GetFileCommand : CommandBase, ICommand<Guid, FileViewModel>
    {
        /// <summary>
        /// Gets or sets the media file service.
        /// </summary>
        /// <value>
        /// The media file service.
        /// </value>
        public IMediaFileService MediaFileService { get; set; }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns>The view model.</returns>
        public virtual FileViewModel Execute(Guid fileId)
        {
            var file = Repository.First<MediaFile>(fileId);
            return new FileViewModel
                {
                    Id = file.Id.ToString(),
                    Title = file.Title,
                    Url = file.PublicUrl,
                    Version = file.Version.ToString(CultureInfo.InvariantCulture),
                    FileName = file.OriginalFileName,
                    FileExtension = file.OriginalFileExtension,
                    FileSize = file.SizeAsText(),
                };
        }
    }
}