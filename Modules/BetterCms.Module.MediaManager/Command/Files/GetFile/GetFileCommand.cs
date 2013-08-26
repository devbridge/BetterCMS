using System;
using System.Globalization;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.MediaManager.ViewModels.File;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

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
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The storage service
        /// </summary>
        private readonly IStorageService storageService;

        /// <summary>
        /// The file service
        /// </summary>
        private readonly IMediaFileService fileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFileCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="configuration">The CMS configuration.</param>
        /// <param name="storageService">The storage service.</param>
        /// <param name="fileService">The file service.</param>
        public GetFileCommand(ITagService tagService, ICmsConfiguration configuration, IStorageService storageService, IMediaFileService fileService)
        {
            this.tagService = tagService;
            this.configuration = configuration;
            this.storageService = storageService;
            this.fileService = fileService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns>The view model.</returns>
        public virtual FileViewModel Execute(Guid fileId)
        {
            var fileQuery = Repository.AsQueryable<MediaFile>().Where(f => f.Id == fileId);

            if (configuration.AccessControlEnabled)
            {
                fileQuery = fileQuery.FetchMany(f => f.AccessRules);
            }

            var file = fileQuery.ToList().First();

            var model = new FileViewModel
                {
                    Id = file.Id.ToString(),
                    Title = file.Title,
                    Description = file.Description,
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
                    AccessControlEnabled = configuration.AccessControlEnabled
                };

            if (configuration.AccessControlEnabled)
            {
                model.UserAccessList = file.AccessRules
                                            .Select(x => new UserAccessViewModel
                                            {
                                                Id = x.Id,
                                                AccessLevel = x.AccessLevel,
                                                ObjectId = fileId,
                                                RoleOrUser = x.RoleOrUser
                                            }).ToList();

                model.Url = fileService.GetDownloadFileUrl(MediaType.File, model.Id.ToGuidOrDefault(), model.Url);
            }


            return model;
        }
    }
}