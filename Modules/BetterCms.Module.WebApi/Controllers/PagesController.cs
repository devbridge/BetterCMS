using System.Linq;
using System.Web.Http;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Operations.Pages.GetPageById;
using BetterCms.Module.Api.Operations.Pages.GetPages;

using Microsoft.Web.Mvc;

using PageModel = BetterCms.Module.Api.Operations.Pages.GetPageById.PageModel;

namespace BetterCms.Module.Api.Controllers
{
    /// <summary>
    /// Blogs API controller.
    /// </summary>
    [ActionLinkArea(ApiModuleDescriptor.WebApiAreaName)]
    public class PagesController : ApiController
    {                
        public GetPageByIdResponse GetPageById([FromUri]GetPageByIdRequest request)
        {
            GetPageByIdResponse response = new GetPageByIdResponse();

            response.Status = "ok";
            response.Data = new PageModel
            {
                                              Id = request.PageId
                                          };

            return response;
        }

        public GetPagesResponse GetPages([FromUri]GetPagesRequest request)
        {
            var response = new GetPagesResponse {Status = "ok"};

            using (var api = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                var serviceRequest = new Pages.Api.DataContracts.GetPagesRequest();
                request.ApplyTo(serviceRequest);

                var pages = api.GetPages(serviceRequest);

                response.Data = new DataListResponse<Operations.Pages.GetPages.PageModel>
                                    {
                                        TotalCount = pages.TotalCount,
                                        Items = pages.Items.Select(page
                                            => new Operations.Pages.GetPages.PageModel
                                                {
                                                    Id = page.Id,
                                                    PageUrl = page.PageUrl,
                                                    Title = page.Title,
                                                    IsPublished = page.Status == PageStatus.Published,
                                                    PublishedOn = page.PublishedOn,
                                                    LayoutId = page.Layout.Id,
                                                    CategoryId = page.Category != null ? page.Category.Id : (System.Guid?)null,
                                                    Version = page.Version
                                                })
                                                .ToArray()
                                    };
            }

            return response;
        }
    }
}
