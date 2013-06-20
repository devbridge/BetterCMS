using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataAccess;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using MvcContrib.Sorting;

namespace BetterCms.Module.MediaManager.Services
{
    public class DefaultMediaHistoryService : IMediaHistoryService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultMediaHistoryService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultMediaHistoryService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the media history.
        /// </summary>
        /// <param name="mediaId">The media id.</param>
        /// <param name="gridOptions">The grid options.</param>
        /// <returns>Media history items.</returns>
        public IList<Media> GetMediaHistory(Guid mediaId, SearchableGridOptions gridOptions)
        {
            var medias = repository
                .AsQueryable<Media>()
                .Where(media => !media.IsDeleted && (media.Id == mediaId || (media.Original != null && media.Original.Id == mediaId)));

            if (!string.IsNullOrEmpty(gridOptions.SearchQuery))
            {
                var searchQuery = (gridOptions.SearchQuery ?? string.Empty).ToLower();
                medias = medias
                    .Where(media => media.Title.Contains(searchQuery) || (media.Original != null && media.Original.Title.Contains(searchQuery)));
            }

            switch (gridOptions.Column)
            {
                case "CreatedOn":
                    medias = gridOptions.Direction == SortDirection.Ascending
                        ? medias.OrderBy(media => media.CreatedOn)
                        : medias.OrderByDescending(media => media.CreatedOn);
                    break;
                case "CreatedByUser":
                    medias = gridOptions.Direction == SortDirection.Ascending
                        ? medias.OrderBy(media => media.CreatedByUser)
                        : medias.OrderByDescending(media => media.CreatedByUser);
                    break;
                case "ArchivedOn":
                    System.Linq.Expressions.Expression<Func<Media, DateTime?>> archivedOnExpression =
                        m => m.Original != null
                            ? m.ModifiedOn
                            : (DateTime?)null;
                    medias = gridOptions.Direction == SortDirection.Ascending
                        ? medias.OrderBy(archivedOnExpression)
                        : medias.OrderByDescending(archivedOnExpression);
                    break;
                case "DisplayedFor":
                    medias = medias.ToList().AsQueryable(); // Note: NHibernate does not support sorting on TimeSpan.
                    System.Linq.Expressions.Expression<Func<Media, TimeSpan?>> displayedForExpression =
                        m => m.Original != null
                            ? m.ModifiedOn - m.CreatedOn
                            : (TimeSpan?)null;
                    medias = gridOptions.Direction == SortDirection.Ascending
                        ? medias.OrderBy(displayedForExpression)
                        : medias.OrderByDescending(displayedForExpression);
                    break;
                case "StatusName":
                    System.Linq.Expressions.Expression<Func<Media, string>> statusNameExpression =
                        m => m.Original != null
                            ? MediaGlobalization.MediaHistory_Status_Archived
                            : MediaGlobalization.MediaHistory_Status_Active;
                    medias = gridOptions.Direction == SortDirection.Ascending
                        ? medias.OrderBy(statusNameExpression)
                        : medias.OrderByDescending(statusNameExpression);
                    break;
                default:
                    medias = medias.OrderBy(media => media.Original).ThenByDescending(o => o.CreatedOn);
                    break;
            }

            medias = medias.AddPaging(gridOptions);

            return medias.ToList();
        }
    }
}