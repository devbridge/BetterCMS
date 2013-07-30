using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects
{
    public class RedirectsService : Service, IRedirectsService
    {
        private readonly IRepository repository;

        public RedirectsService(IRepository repository)
        {
            this.repository = repository;
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
    }
}