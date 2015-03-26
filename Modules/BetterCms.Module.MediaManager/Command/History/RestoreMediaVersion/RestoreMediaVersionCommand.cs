using System;
using System.Linq;

using BetterCms.Module.MediaManager.Helpers;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.Services;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.History.RestoreMediaVersion
{
    public class RestoreMediaVersionCommand : CommandBase, ICommand<RestoreMediaVersionRequest, bool>
    {
        private readonly IMediaImageService imageService;

        public RestoreMediaVersionCommand(IMediaImageService imageService)
        {
            this.imageService = imageService;
        }

        public bool Execute(RestoreMediaVersionRequest request)
        {
            var imageToRevert = Repository
                .AsQueryable<MediaImage>(i => i.Id == request.VersionId)
                .Fetch(f => f.Original)

                .FirstOrDefault();

            if (imageToRevert != null)
            {
                var currentOriginal = Repository
                .AsQueryable<MediaImage>(i => imageToRevert.Original != null && i.Id == imageToRevert.Original.Id)
                .Fetch(f => f.Original)
                .FirstOrDefault();

                if (currentOriginal != null)
                {
                    var archivedImage = imageService.MoveToHistory(currentOriginal);
                    var newOriginalImage = imageService.MakeAsOriginal(imageToRevert, currentOriginal, archivedImage, request.ShouldOverridUrl);
                    Events.MediaManagerEvents.Instance.OnMediaRestored(newOriginalImage);
                }

                return true;
            }
            
            var versionToRevert = Repository.AsQueryable<Media>(p => p.Id == request.VersionId).Fetch(f => f.Original).First();

            var original = versionToRevert.Original;

            if (original != null)
            {
                UnitOfWork.BeginTransaction();
                Repository.Save(original.CreateHistoryItem());
                versionToRevert.CopyDataTo(original, false);
                MediaHelper.SetCollections(Repository, versionToRevert, original);
                original.Original = null;
                original.PublishedOn = DateTime.Now;
                Repository.Save(original);
                UnitOfWork.Commit();

                Events.MediaManagerEvents.Instance.OnMediaRestored(original);

                return true;
            }

            return false;
        }
    }
}