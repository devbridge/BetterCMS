using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    public class RedirectsService : Service, IRedirectsService
    {
        public GetRedirectsResponse Get(GetRedirectsRequest request)
        {
            return new GetRedirectsResponse
                       {
                           Data =
                               new DataListResponse<RedirectModel>
                                   {
                                       TotalCount = 111,
                                       Items =
                                           new List<RedirectModel>
                                               {
                                                   new RedirectModel(),
                                                   new RedirectModel(),
                                                   new RedirectModel()
                                               }
                                   }
                       };
        }
    }
}