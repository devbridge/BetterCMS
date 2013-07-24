using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    public class RedirectService : Service, IRedirectService
    {
        private readonly IRepository repository;

        public RedirectService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetRedirectResponse Get(GetRedirectRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.Redirect>(redirect => redirect.Id == request.RedirectId)
                .Select(redirect => new RedirectModel
                    {
                        Id = redirect.Id,
                        Version = redirect.Version,
                        CreatedBy = redirect.CreatedByUser,
                        CreatedOn = redirect.CreatedOn,
                        LastModifiedBy = redirect.ModifiedByUser,
                        LastModifiedOn = redirect.ModifiedOn,

                        PageUrl = redirect.PageUrl,
                        RedirectUrl = redirect.RedirectUrl,
                    })
                .FirstOne();

            return new GetRedirectResponse
                       {
                           Data = model
                       };
        }
    }
}