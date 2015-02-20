using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

using MvcContrib.Sorting;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultHistoryService : IHistoryService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The list of status names
        /// </summary>
        private readonly IDictionary<ContentStatus, string> statusNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultHistoryService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultHistoryService(IRepository repository)
        {
            this.repository = repository;

            statusNames = new Dictionary<ContentStatus, string>
                              {
                                  { ContentStatus.Archived, PagesGlobalization.ContentStatus_Archived },
                                  { ContentStatus.Draft, PagesGlobalization.ContentStatus_Draft },
                                  { ContentStatus.Preview, PagesGlobalization.ContentStatus_Preview },
                                  { ContentStatus.Published, PagesGlobalization.ContentStatus_Published }
                              };
        }

        /// <summary>
        /// Gets the list with historical content entities.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="gridOptions">The grid options.</param>
        /// <returns>
        /// Historical content entities
        /// </returns>
        public IList<Root.Models.Content> GetContentHistory(Guid contentId, Root.Mvc.Grids.GridOptions.SearchableGridOptions gridOptions)
        {
            var searchQuery = (gridOptions.SearchQuery ?? string.Empty).ToLower();

            var history = new List<Root.Models.Content>();

            var contentsList = repository
                .AsQueryable<Root.Models.Content>()
                .Where(f => f.Id == contentId 
                    && !f.IsDeleted
                    && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived))
                .FetchMany(f => f.History)
                // Load list, because query has FetchMany
                .ToList();

            var content = contentsList.FirstOrDefault();

            if (content != null)
            {
                // Fix for draft: loading it's original content
                if (content.Status == ContentStatus.Draft)
                {
                    var publishedContent = repository
                        .AsQueryable<Root.Models.Content>()
                        .Where(f => f == content.Original
                            && !f.IsDeleted
                            && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived))
                        .FetchMany(f => f.History)
                        .ToList().FirstOrDefault();

                    if (publishedContent != null)
                    {
                        content = publishedContent;
                    }
                    else
                    {
                        // If draft has no original content, adding itself to history
                        history.Add(content);
                    }
                }

                if (content.Status == ContentStatus.Published && ContainsSearchQuery(content, searchQuery))
                {
                    history.Add(content);
                }

                history.AddRange(content.History.Where(c => IsValidHistoricalContent(c) && ContainsSearchQuery(c, searchQuery)));
            }

            // Order
            history = AddSortAndPaging(history, gridOptions);

            return history;
        }

        /// <summary>
        /// Gets the name of the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>The name of the status</returns>
        public string GetStatusName(ContentStatus status)
        {
            if (statusNames.ContainsKey(status))
            {
                return statusNames[status];
            }
            return null;
        }

        /// <summary>
        /// Determines whether content is valid historical content.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <returns>
        ///   <c>true</c> if content is valid historical content; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidHistoricalContent(IHistorical f)
        {
            return !f.IsDeleted && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived);
        }

        /// <summary>
        /// Determines whether content contains search query.
        /// </summary>
        /// <param name="f">The f.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <returns>
        ///   <c>true</c> if [contains search query] [the specified f]; otherwise, <c>false</c>.
        /// </returns>
        private bool ContainsSearchQuery(IHistorical f, string searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var statusName = GetStatusName(f.Status).ToLower();

                return (f.Status == ContentStatus.Published && !string.IsNullOrEmpty(f.PublishedByUser) && f.PublishedByUser.ToLower().Contains(searchQuery))
                    || f.CreatedByUser.ToLower().Contains(searchQuery)
                    || (!string.IsNullOrEmpty(statusName) && statusName.Contains(searchQuery));
            }
            return true;
        }

        /// <summary>
        /// Adds the sort and paging.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <param name="gridOptions">The grid options.</param>
        /// <returns></returns>
        private List<Root.Models.Content> AddSortAndPaging(List<Root.Models.Content> history, Root.Mvc.Grids.GridOptions.SearchableGridOptions gridOptions)
        {
            if (string.IsNullOrWhiteSpace(gridOptions.Column))
            {
                history = history.AsQueryable().OrderBy(o => o.Status).ThenByDescending(o => o.CreatedOn).AddPaging(gridOptions).ToList();
            }
            else if (gridOptions.Column == "StatusName")
            {
                history = (gridOptions.Direction == SortDirection.Ascending)
                              ? history.AsQueryable().OrderBy(o => GetStatusName(o.Status)).AddPaging(gridOptions).ToList()
                              : history.AsQueryable().OrderByDescending(o => GetStatusName(o.Status)).AddPaging(gridOptions).ToList();
            }
            else if (gridOptions.Column == "DisplayedFor")
            {
                System.Linq.Expressions.Expression<Func<Root.Models.Content, TimeSpan?>> orderExpression =
                    (c) => c.Status == ContentStatus.Archived && c.PublishedOn != null
                        ? c.CreatedOn - c.PublishedOn.Value
                        : (TimeSpan?)null;

                history = (gridOptions.Direction == SortDirection.Ascending)
                              ? history.AsQueryable().OrderBy(orderExpression).AddPaging(gridOptions).ToList()
                              : history.AsQueryable().OrderByDescending(orderExpression).AddPaging(gridOptions).ToList();
            }
            else if (gridOptions.Column == "ArchivedOn")
            {
                System.Linq.Expressions.Expression<Func<Root.Models.Content, DateTime?>> orderExpression =
                    (c) => c.Status == ContentStatus.Archived
                        ? c.CreatedOn
                        : (DateTime?)null;

                history = (gridOptions.Direction == SortDirection.Ascending)
                              ? history.AsQueryable().OrderBy(orderExpression).AddPaging(gridOptions).ToList()
                              : history.AsQueryable().OrderByDescending(orderExpression).AddPaging(gridOptions).ToList();
            }
            else
            {
                history = history.AsQueryable().AddSortingAndPaging(gridOptions).ToList();
            }

            return history;
        }
    }
}