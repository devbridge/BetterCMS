using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Extensions;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.Upload.CheckFileStatuses
{
    public class CheckFilesStatusesCommand : CommandBase, ICommand<List<string>, IList<MediaFileViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of media file view models
        /// </returns>
        public IList<MediaFileViewModel> Execute(List<string> request)
        {
            IList<MediaFileViewModel> files = null;
            List<Guid> ids = new List<Guid>(request.Count);

            foreach (var id in request)
            {
                var guid = id.ToGuidOrDefault();
                if (!guid.HasDefaultValue())
                {
                    ids.Add(guid);
                }
            }

            if (ids.Count > 0)
            {
                files = Repository
                    .AsQueryable<MediaFile>()
                    .Where(f => ids.Contains(f.Id))
                    .ToList()
                    .Select(f => new MediaFileViewModel
                                     {
                                         Id = f.Id,
                                         IsProcessing = f.GetIsProcessing(),
                                         IsFailed = f.GetIsFailed()
                                     })
                    .ToList();
            }

            return files ?? new List<MediaFileViewModel>();
        }
    }
}