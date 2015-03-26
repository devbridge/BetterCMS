using System;
using System.Globalization;
using System.Linq;

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Core.Security;
using BetterCms.Core.Services.Storage;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.MediaManager.ViewModels.File;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

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
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The category service
        /// </summary>
        private ICategoryService categoryService;
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFileCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="configuration">The CMS configuration.</param>
        /// <param name="storageService">The storage service.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        public GetFileCommand(ITagService tagService, ICmsConfiguration configuration, IStorageService storageService, IMediaFileService fileService, IMediaFileUrlResolver fileUrlResolver, ICategoryService categoryService)
        {
            this.tagService = tagService;
            this.configuration = configuration;
            this.storageService = storageService;
            this.fileService = fileService;
            this.fileUrlResolver = fileUrlResolver;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <returns>The view model.</returns>
        public virtual FileViewModel Execute(Guid fileId)
        {
            var fileQuery = Repository.AsQueryable<MediaFile>().Where(f => f.Id == fileId && !f.IsDeleted);

            if (configuration.Security.AccessControlEnabled)
            {
                fileQuery = fileQuery.FetchMany(f => f.AccessRules);
            }

            var file = fileQuery.ToList().FirstOne();

            var model = new FileViewModel
                {
                    Id = file.Id.ToString(),
                    Title = file.Title,
                    Description = file.Description,
                    Url = fileUrlResolver.EnsureFullPathUrl(file.PublicUrl),
                    Version = file.Version.ToString(CultureInfo.InvariantCulture),
                    FileName = file.OriginalFileName,
                    FileExtension = file.OriginalFileExtension,
                    FileSize = file.SizeAsText(),
                    Tags = tagService.GetMediaTagNames(fileId),
                    Image = file.Image == null || file.Image.IsDeleted ? null :
                        new ImageSelectorViewModel
                        {
                            ImageId = file.Image.Id,
                            ImageVersion = file.Image.Version,
                            ImageUrl = fileUrlResolver.EnsureFullPathUrl(file.Image.PublicUrl),
                            ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(file.Image.PublicThumbnailUrl),
                            ImageTooltip = file.Image.Caption,
                            FolderId = file.Image.Folder != null ? file.Image.Folder.Id : (Guid?)null
                        },
                    AccessControlEnabled = configuration.Security.AccessControlEnabled
                };

            model.Categories = categoryService.GetSelectedCategories<Media, MediaCategory>(fileId);

            if (configuration.Security.AccessControlEnabled)
            {
                AccessControlService.DemandAccess(file, Context.Principal, AccessLevel.Read);

                model.UserAccessList = file.AccessRules.Select(f => new UserAccessViewModel(f)).ToList();
                model.Url = fileService.GetDownloadFileUrl(MediaType.File, model.Id.ToGuidOrDefault(), model.Url);

                SetIsReadOnly(model, ((IAccessSecuredObject)file).AccessRules);
            }

            model.CategoriesFilterKey = file.GetCategorizableItemKey();

            return model;
        }
    }
}