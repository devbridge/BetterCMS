using System;
using System.Globalization;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
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
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFileCommand"/> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        public GetFileCommand(ITagService tagService)
        {
            this.tagService = tagService;
        }

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
                    Tags = tagService.GetMediaTagNames(fileId),
                    Image = file.Image == null ? null :
                        new ImageSelectorViewModel
                        {
                            ImageId = file.Image.Id,
                            ImageVersion = file.Image.Version,
                            ImageUrl = file.Image.PublicUrl,
                            ThumbnailUrl = file.Image.PublicThumbnailUrl,
                            ImageTooltip = file.Image.Caption
                        },
                };
        }
    }
}