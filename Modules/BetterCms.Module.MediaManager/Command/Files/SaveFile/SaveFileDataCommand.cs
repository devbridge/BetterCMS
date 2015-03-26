using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.File;

using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Files.SaveFile
{
    /// <summary>
    /// Command to save image properties.
    /// </summary>
    public class SaveFileDataCommand : CommandBase, ICommandIn<FileViewModel>
    {
        /// <summary>
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The category service
        /// </summary>
        private ICategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDataCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        public SaveFileDataCommand(ITagService tagService, ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService, ICategoryService categoryService)
        {
            this.tagService = tagService;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Execute(FileViewModel request)
        {
//            var mediaFile = Repository.First<MediaFile>(request.Id.ToGuidOrDefault());

            var mediaFile = Repository.AsQueryable<MediaFile>().Where(mf => mf.Id == request.Id.ToGuidOrDefault()).ToList().First();
            UnitOfWork.BeginTransaction();

            var histItem = (MediaFile)mediaFile.CreateHistoryItem();
            histItem.SaveUnsecured = true;
            Repository.Save(histItem);

            mediaFile.PublishedOn = DateTime.UtcNow;
            mediaFile.Title = request.Title;
            mediaFile.Description = request.Description;
            mediaFile.Version = request.Version.ToIntOrDefault();
            mediaFile.Image = request.Image != null && request.Image.ImageId.HasValue ? Repository.AsProxy<MediaImage>(request.Image.ImageId.Value) : null;

            categoryService.CombineEntityCategories<Media, MediaCategory>(mediaFile, request.Categories); 

            Repository.Save(mediaFile);

            // Save tags.
            IList<Root.Models.Tag> newTags;
            tagService.SaveMediaTags(mediaFile, request.Tags, out newTags);

            // Save user access if enabled.
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                mediaFile.AccessRules.RemoveDuplicateEntities();

                var accessRules = request.UserAccessList != null ? request.UserAccessList.Cast<IAccessRule>().Distinct().ToList() : null;
                accessControlService.UpdateAccessControl(mediaFile, accessRules);
            }

            UnitOfWork.Commit();

            // Notify.
            Events.MediaManagerEvents.Instance.OnMediaFileUpdated(mediaFile);
        }
    }
}
