using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    public class PageContentsService : Service, IPageContentsService
    {
        public GetPageContentsResponse Get(GetPageContentsRequest request)
        {
            // TODO: need implementation
            return new GetPageContentsResponse
                       {
                           Data =
                               new DataListResponse<PageContentModel>
                                   {
                                       Items =
                                           new List<PageContentModel>
                                               {
                                                   new PageContentModel(),
                                                   new PageContentModel(),
                                                   new PageContentModel()
                                               },
                                       TotalCount = 27
                                   }
                       };
        }
    }
}