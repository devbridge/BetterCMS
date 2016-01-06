using System;
using System.Linq.Expressions;
using System.Web;

using BetterModules.Core.DataAccess;
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

        protected override System.Collections.Generic.IEnumerable<Guid> GetDeniedMedias(ViewModels.MediaManager.MediaManagerViewModel request)
        {
            return AccessControlService.GetDeniedObjects<MediaFile>();
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