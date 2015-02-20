using System.Linq;

using BetterModules.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Redirects.Redirect;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    public class RedirectsService : Service, IRedirectsService
    {
        private readonly IRepository repository;
        
        private readonly IRedirectService redirectService;

        public RedirectsService(IRepository repository, IRedirectService redirectService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
        }

        public GetRedirectsResponse Get(GetRedirectsRequest request)
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