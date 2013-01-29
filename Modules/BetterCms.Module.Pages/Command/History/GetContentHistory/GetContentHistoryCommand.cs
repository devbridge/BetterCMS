using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.History.GetContentHistory
{
    /// <summary>
    /// Command to load a list of the content history versions.
    /// </summary>
    public class GetContentHistoryCommand : CommandBase, ICommand<GetContentHistoryRequest, PageContentHistoryViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public PageContentHistoryViewModel Execute(GetContentHistoryRequest request)
        {
            var history = new List<PageContentHistoryItem>();
            request.SetDefaultSortingOptions("CreatedOn", true);            

            var query = Repository
                .AsQueryable<Root.Models.Content>()
                .Where(f => f.Id == request.ContentId && !f.IsDeleted)
                .Where(f => f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                // TODO: search by status
                query = query.Where(q => q.CreatedByUser.Contains(request.SearchQuery) 
                    || q.PublishedByUser.Contains(request.SearchQuery));
            }

            var content = query
                .FetchMany(f => f.History)
                .ToFuture()
                .ToList().FirstOrDefault();

            if (content != null)
            {
                if (content.Status == ContentStatus.Published)
                {
                    history.Add(Convert(content));
                }

                history.AddRange(content.History.Where(IsValidHistoricalContent).Select(Convert));     
            }

            history = history.AsQueryable().AddSortingAndPaging(request).ToList();

            return new PageContentHistoryViewModel(history, request, history.Count, request.ContentId, request.PageContentId);
        }

        private bool IsValidHistoricalContent(IHistorical f)
        {
            return !f.IsDeleted && (f.Status == ContentStatus.Published || f.Status == ContentStatus.Draft || f.Status == ContentStatus.Archived);
        }

        private PageContentHistoryItem Convert(IHistorical content)
        {
            return new PageContentHistoryItem
                       {
                           Id = content.Id,
                           Version = content.Version,                           
                           Status = content.Status,
                           ArchivedByUser = content.Status == ContentStatus.Archived ? content.CreatedByUser : null,
                           ArchivedOn = content.Status == ContentStatus.Archived ? content.CreatedOn : (DateTime?)null,
                           DisplayedFor = content.Status == ContentStatus.Archived && content.PublishedOn != null
                                   ? content.CreatedOn - content.PublishedOn.Value
                                   : (TimeSpan?)null,
                           PublishedByUser = content.Status == ContentStatus.Published ? content.PublishedByUser : null,
                           PublishedOn = content.Status == ContentStatus.Published ? content.PublishedOn : null,
                           CreatedByUser = content.CreatedByUser,
                           CreatedOn = content.CreatedOn
                       };
        }        
    }
}