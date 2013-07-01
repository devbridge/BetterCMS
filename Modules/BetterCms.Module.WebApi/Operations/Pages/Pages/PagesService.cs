using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    public class PagesService : Service, IPagesService
    {
        public GetPagesResponse Get(GetPagesRequest request)
        {
            return new GetPagesResponse
                       {
                           Data = new DataListResponse<PageModel>
                                      {
                                          Items = new List<PageModel>
                                                      {
                                                          new PageModel(),
                                                          new PageModel(),
                                                          new PageModel()
                                                      }
                                      }
                       };
        }
    }
}