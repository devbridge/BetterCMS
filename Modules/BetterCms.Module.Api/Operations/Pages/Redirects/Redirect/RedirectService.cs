using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Extensions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    public class RedirectService : Service, IRedirectService
    {
        private readonly IRepository repository;

        private readonly Module.Pages.Services.IRedirectService redirectService;

        public RedirectService(IRepository repository, Module.Pages.Services.IRedirectService redirectService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
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

        public PutRedirectResponse Put(PutRedirectRequest request)
        {
            var model = request.Data.ToServiceModel();
            if (request.Id.HasValue)
            {
                model.Id = request.Id.Value;
            }

            var redirect = redirectService.SaveRedirect(model, true);

            return new PutRedirectResponse { Data = redirect.Id };
        }

        public DeleteRedirectResponse Delete(DeleteRedirectRequest request)
        {
            var result = redirectService.DeleteRedirect(request.Id, request.Data.Version);

            return new DeleteRedirectResponse { Data = result };
        }
    }
}