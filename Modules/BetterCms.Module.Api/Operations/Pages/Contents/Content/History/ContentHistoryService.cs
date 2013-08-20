using System.Linq;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using ServiceStack.ServiceInterface;

using CoreContentStatus = BetterCms.Core.DataContracts.Enums.ContentStatus;

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
            var results = historyService.GetContentHistory(request.ContentId, new SearchableGridOptions())
                .AsQueryable()
                .OrderBy(history => history.CreatedOn)
                .Select(history => new
                    {
                        Type = history.GetType(),
                        Model = new HistoryContentModel
                            {
                                Id = history.Id,
                                Version = history.Version,
                                CreatedBy = history.CreatedByUser,
                                CreatedOn = history.CreatedOn,
                                LastModifiedBy = history.ModifiedByUser,
                                LastModifiedOn = history.ModifiedOn,

                                OriginalContentId = history.Original != null ? history.Original.Id : (System.Guid?)null,
                                PublishedOn = history.Status == CoreContentStatus.Published ? history.PublishedOn : null,
                                PublishedByUser = history.Status == CoreContentStatus.Published ? history.PublishedByUser : null,
                                ArchivedOn = history.Status == CoreContentStatus.Archived ? history.CreatedOn : (System.DateTime?)null,
                                ArchivedByUser = history.Status == CoreContentStatus.Archived ? history.CreatedByUser : null,
                                Status = (ContentStatus)((int)history.Status)
                            }
                    })
                .ToList();

            // Set content types
            results.ForEach(item => item.Model.ContentType = item.Type.ToContentTypeString());

            return new GetContentHistoryResponse
                       {
                           Data = results.Select(item => item.Model).ToList()
                       };
        }
    }
}