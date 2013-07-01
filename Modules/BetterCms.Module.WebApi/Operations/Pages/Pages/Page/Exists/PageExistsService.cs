using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    public class PageExistsService : Service, IPageExistsService
    {
        public PageExistsResponse Get(PageExistsRequest request)
        {
            // TODO
            return new PageExistsResponse
                       {
                           Data = new PageExistsModel
                                      {
                                          Exists = false, 
                                          PageId = System.Guid.NewGuid()
                                      }
                       };
        }
    }
}