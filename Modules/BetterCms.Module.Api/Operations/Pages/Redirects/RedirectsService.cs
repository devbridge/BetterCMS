using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    [RoutePrefix("bcms-api")]       
    public class RedirectsController : ApiController, IRedirectsService
    {
        private readonly IRepository repository;
        
        private readonly IRedirectService redirectService;

        public RedirectsController(IRepository repository, IRedirectService redirectService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
        }

        [Route("redirects/")]
        public GetRedirectsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetRedirectsRequest request)
        {
            request.Data.SetDefaultOrder("PageUrl");

            var listResponse = repository
                .AsQueryable<Module.Pages.Models.Redirect>()
                .Select(redirect => new RedirectModel()
                    {
                        Id = redirect.Id,
                        Version = redirect.Version,
                        CreatedBy = redirect.CreatedByUser,
                        CreatedOn = redirect.CreatedOn,
                        LastModifiedBy = redirect.ModifiedByUser,
                        LastModifiedOn = redirect.ModifiedOn,

                        PageUrl = redirect.PageUrl,
                        RedirectUrl = redirect.RedirectUrl
                    })
                .ToDataListResponse(request);

            return new GetRedirectsResponse
                       {
                           Data = listResponse
                       };
        }

        [Route("redirects/")]
        public PostRedirectResponse Post(PostRedirectRequest request)
        {
            var result = redirectService.Put(new PutRedirectRequest
                {
                    Data = request.Data,
                    User = request.User
                });

            return new PostRedirectResponse { Data = result.Data };
        }
    }
}