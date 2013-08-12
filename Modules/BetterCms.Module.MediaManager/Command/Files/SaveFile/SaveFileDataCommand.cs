using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.AccessControl.Models;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
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
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveFileDataCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        public SaveFileDataCommand(ITagService tagService, ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService)
        {
            this.tagService = tagService;
            this.cmsConfiguration = cmsConfiguration;
        }

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
            mediaFile.Description = request.Description;
            mediaFile.Version = request.Version.ToIntOrDefault();
            mediaFile.Image = request.Image != null && request.Image.ImageId.HasValue ? Repository.AsProxy<MediaImage>(request.Image.ImageId.Value) : null;
            Repository.Save(mediaFile);

            // Save tags
            IList<Root.Models.Tag> newTags;
            tagService.SaveMediaTags(mediaFile, request.Tags, out newTags);

            // Save user access if enabled:
            if (cmsConfiguration.AccessControlEnabled)
            {
                request.UserAccessList.ForEach(x => x.ObjectId = mediaFile.Id);

                var entities = Repository.AsQueryable<UserAccess>()
                                    .Where(x => x.ObjectId == mediaFile.Id)
                                    .ToList();

                var entitiesToDelete = entities.Where(x => request.UserAccessList.All(model => model.RoleOrUser != x.RoleOrUser)).ToList();

                var entitesToAdd = request.UserAccessList
                                      .Where(x => entities.All(entity => entity.RoleOrUser != x.RoleOrUser))
                                      .Select(ModelToEntity)
                                      .ToList();

                var entitiesToUpdate = GetEntitiesToUpdate(request, entities);

                entitiesToDelete.ForEach(entity => Repository.Delete(entity));
                entitiesToUpdate.ForEach(entity => Repository.Save(entity));
                entitesToAdd.ForEach(entity => Repository.Save(entity));
            }

            UnitOfWork.Commit();

            // Notify.
            Events.MediaManagerEvents.Instance.OnMediaFileUpdated(mediaFile);
        }

        private static List<UserAccess> GetEntitiesToUpdate(FileViewModel request, List<UserAccess> entities)
        {
            var entitiesToUpdate = new List<UserAccess>();

            foreach (var entity in entities)
            {
                // Find user access object with the same Role and diferrent AccessLevel:
                var userAccess = request.UserAccessList.FirstOrDefault(x => x.RoleOrUser == entity.RoleOrUser && x.AccessLevel != entity.AccessLevel);

                // If found, add to updatables list:
                if (userAccess != null)
                {
                    entity.AccessLevel = userAccess.AccessLevel;
                    entitiesToUpdate.Add(entity);
                }
            }

            return entitiesToUpdate;
        }

        private static UserAccess ModelToEntity(UserAccessViewModel model)
        {
            return new UserAccess
                       {
                           ObjectId = model.ObjectId,
                           RoleOrUser = model.RoleOrUser,
                           AccessLevel = model.AccessLevel
                       };
        }
    }
}
