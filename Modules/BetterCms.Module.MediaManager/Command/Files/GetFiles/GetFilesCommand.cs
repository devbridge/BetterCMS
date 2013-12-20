using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

using BetterCms.Core.DataAccess;
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

        /// <summary>
        /// Appends the search filter.
        /// </summary>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <returns>
        /// Appended search filter
        /// </returns>
        protected override Expression<Func<Media, bool>> AppendSearchFilter(Expression<Func<Media, bool>> searchFilter, string searchQuery)
        {
            var searcQueryDecoded = HttpUtility.UrlDecode(searchQuery);

            return searchFilter.Or(m => (m is MediaFile
                && (((MediaFile)m).PublicUrl.Contains(searchQuery)) || ((MediaFile)m).PublicUrl.Contains(searcQueryDecoded)));
        }
    }
}