using System.Linq;
using System.Web.Http;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.WebApi.Helpers;
using BetterCms.Module.WebApi.Models.Pages;
using BetterCms.Module.WebApi.Models.Pages.GetPageById;
using BetterCms.Module.WebApi.Models.Pages.GetPages;

using Microsoft.Web.Mvc;

using GetPagesServiceRequest = BetterCms.Module.Pages.Api.DataContracts.GetPagesRequest;

namespace BetterCms.Module.WebApi.Controllers
{
    /// <summary>
    /// Blogs API controller.
    /// </summary>
    [ActionLinkArea(WebApiModuleDescriptor.WebApiAreaName)]
    public class PagesController : ApiController
    {                
        /*public GetPageByIdResponse GetPageById([FromUri]GetPageByIdRequest request)
        {
            GetPageByIdResponse response = new GetPageByIdResponse();

            response.Status = "ok";
            response.Data = new PageModel {
                                              Id = request.PageId.ToGuidOrDefault()
                                          };

            return response;
        }*/

        public GetPagesResponse GetPages([FromUri]GetPagesRequest request)
        {
            var response = new GetPagesResponse {Status = "ok"};

            using (var api = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                var serviceRequest = new GetPagesServiceRequest();
                request.ApplyTo(serviceRequest);

                var pages = api.GetPages(serviceRequest);

                response.Data = new DataListResponse<PageModel>
                                    {
                                        TotalCount = pages.TotalCount,
                                        Items = pages.Items.Select(page
                                            => new PageModel
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
