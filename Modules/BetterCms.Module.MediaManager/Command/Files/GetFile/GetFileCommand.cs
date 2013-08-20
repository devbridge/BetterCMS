using System;
using System.Globalization;
using System.Linq;

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
        private readonly ICmsConfiguration cmsConfiguration;

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
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="storageService">The storage service.</param>
        /// <param name="fileService">The file service.</param>
        public GetFileCommand(ITagService tagService, ICmsConfiguration cmsConfiguration, IStorageService storageService, IMediaFileService fileService)
        {
            this.tagService = tagService;
            this.cmsConfiguration = cmsConfiguration;
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
            var file = Repository.First<MediaFile>(fileId);

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
                    AccessControlEnabled = cmsConfiguration.AccessControlEnabled
                };

            if (cmsConfiguration.AccessControlEnabled)
            {
                model.UserAccessList = Repository.AsQueryable<UserAccess>()
                                            .Where(x => x.ObjectId == fileId)
                                            .OrderBy(x => x.RoleOrUser)
                                            .Select(x => new UserAccessViewModel
                                            {
                                                Id = x.Id,
                                                AccessLevel = x.AccessLevel,
                                                ObjectId = x.ObjectId,
                                                RoleOrUser = x.RoleOrUser
                                            }).ToList();

                model.Url = fileService.GetDownloadFileUrl(MediaType.File, model.Id.ToGuidOrDefault(), model.Url);
            }


            return model;
        }
    }
}