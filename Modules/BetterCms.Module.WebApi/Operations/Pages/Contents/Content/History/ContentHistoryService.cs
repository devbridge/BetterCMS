using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    public class ContentHistoryService : Service, IContentHistoryService
    {
        public GetContentHistoryResponse Get(GetContentHistoryRequest request)
        {
            // TODO: need implementation
            return new GetContentHistoryResponse
                       {
                           Data =
                               new DataListResponse<HistoryContentModel>
                                   {
                                       TotalCount = 257,
                                       Items =
                                           new List<HistoryContentModel>
                                               {
                                                   new HistoryContentModel(),
                                                   new HistoryContentModel(),
                                                   new HistoryContentModel()
                                               }
                                   }
                       };
        }
    }
}