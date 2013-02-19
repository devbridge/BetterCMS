using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.GetContentHistory
{
    /// <summary>
    /// Command to load a list of the content history versions.
    /// </summary>
    public class GetContentHistoryCommand : CommandBase, ICommand<GetContentHistoryRequest, ContentHistoryViewModel>
    {
        /// <summary>
        /// The list of status names
        /// </summary>
        private readonly IDictionary<ContentStatus, string> statusNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContentHistoryCommand" /> class.
        /// </summary>
        public GetContentHistoryCommand()
        {
            statusNames = new Dictionary<ContentStatus, string>
                              {
                                  {ContentStatus.Archived, PagesGlobalization.ContentStatus_Archived},
                                  {ContentStatus.Draft, PagesGlobalization.ContentStatus_Draft},
                                  {ContentStatus.Preview, PagesGlobalization.ContentStatus_Preview},
                                  {ContentStatus.Published, PagesGlobalization.ContentStatus_Published}
                              };
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ContentHistoryViewModel Execute(GetContentHistoryRequest request)
        {
            var searchQuery = (request.SearchQuery ?? string.Empty).ToLower();

            var history = new List<ContentHistoryItem>();

            var contentFutureQuery = Repository
                .AsQueryable<Root.Models.Content>()
                .Where(f => f.Id == request.ContentId && !f.IsDeleted)
                .Where(f => f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived)
                .FetchMany(f => f.History)
                .ToFuture();

            var content = contentFutureQuery.ToList().FirstOrDefault();

            if (content != null)
            {
                // Fix for draft: loading it's original content
                if (content.Status == ContentStatus.Draft)
                {
                    var publishedContent = Repository
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
                        history.Add(Convert(content));
                    }
                }

                if (content.Status == ContentStatus.Published && ContainsSearchQuery(content, searchQuery))
                {
                    history.Add(Convert(content));
                }

                history.AddRange(content.History.Where(c => IsValidHistoricalContent(c) && ContainsSearchQuery(c, searchQuery)).Select(Convert));
            }

            if (string.IsNullOrWhiteSpace(request.Column))
            {
                history = history.AsQueryable().OrderBy(o => o.Status).ThenByDescending(o => o.CreatedOn).AddPaging(request).ToList();
            }
            else
            {
                history = history.AsQueryable().AddSortingAndPaging(request).ToList();
            }

            return new ContentHistoryViewModel(history, request, history.Count, request.ContentId);
        }

        private bool IsValidHistoricalContent(IHistorical f)
        {
            return !f.IsDeleted && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived);
        }

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

        private ContentHistoryItem Convert(IHistorical content)
        {
            return new ContentHistoryItem
                       {
                           Id = content.Id,
                           Version = content.Version,
                           StatusName = GetStatusName(content.Status),
                           Status = content.Status,
                           ArchivedByUser = content.Status == ContentStatus.Archived ? content.CreatedByUser : null,
                           ArchivedOn = content.Status == ContentStatus.Archived ? content.CreatedOn : (DateTime?)null,
                           DisplayedFor = content.Status == ContentStatus.Archived && content.PublishedOn != null
                                   ? content.CreatedOn - content.PublishedOn.Value
                                   : (TimeSpan?)null,
                           PublishedByUser = content.Status == ContentStatus.Published || content.Status == ContentStatus.Archived ? content.PublishedByUser : null,
                           PublishedOn = content.Status == ContentStatus.Published || content.Status == ContentStatus.Archived ? content.PublishedOn : null,
                           CreatedOn = content.CreatedOn
                       };
        }

        private string GetStatusName(ContentStatus status)
        {
            if (statusNames.ContainsKey(status))
            {
                return statusNames[status];
            }
            return null;
        }
    }
}