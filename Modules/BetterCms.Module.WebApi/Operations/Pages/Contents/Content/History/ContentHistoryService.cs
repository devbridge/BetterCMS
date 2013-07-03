using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    public class ContentHistoryService : Service, IContentHistoryService
    {
        private readonly IHistoryService historyService;

        public ContentHistoryService(IHistoryService historyService)
        {
            this.historyService = historyService;
        }

        public GetContentHistoryResponse Get(GetContentHistoryRequest request)
        {
            var dataListResponse = historyService.GetContentHistory(request.ContentId, new SearchableGridOptions())
                .AsQueryable()
                .OrderBy(history => history.CreatedOn)
                .Select(history => new HistoryContentModel
                    {
                        Id = history.Id,
                        Version = history.Version,
                        CreatedBy = history.CreatedByUser,
                        CreatedOn = history.CreatedOn,
                        LastModifiedBy = history.ModifiedByUser,
                        LastModifiedOn = history.ModifiedOn,

                        // TODO: ContentType = ??? <- need to return content type
                        OriginalContentId = history.Original != null ? history.Original.Id : (System.Guid?)null,
                        PublishedOn = history.Status == ContentStatus.Published ? history.PublishedOn : null,
                        PublishedByUser = history.Status == ContentStatus.Published ? history.PublishedByUser : null,
                        ArchivedOn = history.Status == ContentStatus.Archived ? history.CreatedOn : (System.DateTime?)null,
                        ArchivedByUser = history.Status == ContentStatus.Archived ? history.CreatedByUser : null,
                        // TODO: DisplayedFor result is very interesting! - maybe need to change to long ??
                        DisplayedFor = history.Status == ContentStatus.Archived && history.PublishedOn != null
                                ? history.CreatedOn - history.PublishedOn.Value
                                : (System.TimeSpan?)null,
                        Status = history.Status
                    })
                .ToList();

            return new GetContentHistoryResponse
                       {
                           Data = dataListResponse
                       };
        }
    }
}