using System.Linq;

using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Security;
using BetterCms.Module.MediaManager.Command.MediaManager;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.MediaManager.Command.Files.GetFiles
{
    public class GetFilesCommand : GetMediaItemsCommandBase<MediaFile>
    {
        /// <summary>
        /// Gets the type of the current media items.
        /// </summary>
        /// <value>
        /// The type of the current media items.
        /// </value>
        protected override MediaType MediaType
        {
            get { return MediaType.File; }
        }

        protected override System.Collections.Generic.IEnumerable<System.Guid> GetDeniedMedias(ViewModels.MediaManager.MediaManagerViewModel request)
        {
            var query = Repository.AsQueryable<MediaFile>()
                            .Where(f => f.AccessRules.Any(b => b.AccessLevel == AccessLevel.Deny))
                            .FetchMany(f => f.AccessRules);

            var list = query.ToList().Distinct();
            var principle = SecurityService.GetCurrentPrincipal();

            foreach (var file in list)
            {
                var accessLevel = AccessControlService.GetAccessLevel(file, principle);
                if (accessLevel == AccessLevel.Deny)
                {
                    yield return file.Id;
                }
            }
        }
    }
}