using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
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

            switch (gridOptions.Column)
            {
                case "PublishedOn":
                    medias = gridOptions.Direction == SortDirection.Ascending
                        ? medias.OrderBy(media => media.PublishedOn)
                        : medias.OrderByDescending(media => media.PublishedOn);
                    break;
                case "PublishedByUser":
                    medias = gridOptions.Direction == SortDirection.Ascending
                        ? medias.OrderBy(media => media.ModifiedByUser)
                        : medias.OrderByDescending(media => media.ModifiedByUser);
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
                            ? m.ModifiedOn - m.PublishedOn
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

            var searchQuery = (gridOptions.SearchQuery ?? string.Empty).ToLower();
            medias = medias
                .ToList()
                .Where(media => ContainsSearchQuery(media, searchQuery))
                .AsQueryable()
                .AddPaging(gridOptions);

            return medias.ToList();
        }

        private bool ContainsSearchQuery(Media media, string searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var statusName = media.Original != null ? MediaGlobalization.MediaHistory_Status_Archived : MediaGlobalization.MediaHistory_Status_Active;

                return (media.Original == null && media.ModifiedByUser.ToLower().Contains(searchQuery))
                    || media.CreatedByUser.ToLower().Contains(searchQuery)
                    || (!string.IsNullOrEmpty(statusName) && statusName.ToLower().Contains(searchQuery));
            }
            return true;
        }
    }
}