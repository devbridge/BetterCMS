using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.PageExists
{
    public class PageExistsService : Service
    {
        public PageExistsResponse Get(PageExistsRequest request)
        {
            return new PageExistsResponse {
                                              Data = new PageExistsModel {
                                                                             Exists = false,
                                                                             PageId = null
                                                                         }
                                          };
        }
    }
}