using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Pages;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [RoutePrefix("bcms-api")]
    public class RedirectController : ApiController, IRedirectService
    {
        private readonly IRepository repository;

        private readonly Module.Pages.Services.IRedirectService redirectService;

        public RedirectController(IRepository repository, Module.Pages.Services.IRedirectService redirectService)
        {
            this.repository = repository;
            this.redirectService = redirectService;
        }

        [Route("redirects/{RedirectId}")]
        public GetRedirectResponse Get([ModelBinder(typeof(JsonModelBinder))]GetRedirectRequest request)
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

        [Route("redirects/{Id}")]
        [UrlPopulator]
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

        [Route("redirects/{Id}")]
        [UrlPopulator]
        public DeleteRedirectResponse Delete(DeleteRedirectRequest request)
        {
            var result = redirectService.DeleteRedirect(request.Id, request.Data.Version);

            return new DeleteRedirectResponse { Data = result };
        }
    }
}