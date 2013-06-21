using System.Web.Http;
using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.WebApi.Models.Pages;
using BetterCms.Module.WebApi.Models.Pages.GetPageById;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.WebApi.Controllers
{
    /// <summary>
    /// Blogs API controller.
    /// </summary>
    [ActionLinkArea(WebApiModuleDescriptor.WebApiAreaName)]
    public class PagesController : ApiController
    {                
        public GetPageByIdResponse GetPageById([FromUri]GetPageByIdRequest request)
        {
            GetPageByIdResponse response = new GetPageByIdResponse();

            response.Status = "ok";
            response.Data = new PageModel {
                                              Id = request.PageId.ToGuidOrDefault()
                                          };

            return response;
        }
    }
}
