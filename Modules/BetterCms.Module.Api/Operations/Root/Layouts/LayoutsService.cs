using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    [RoutePrefix("bcms-api")]
    public class LayoutsController : ApiController, ILayoutsService
    {
        private readonly IRepository repository;
        
        private readonly ILayoutService layoutService;

        public LayoutsController(IRepository repository, ILayoutService layoutService)
        {
            this.repository = repository;
            this.layoutService = layoutService;
        }

        [Route("layouts")]
        public GetLayoutsResponse Get([ModelBinder(typeof(JsonModelBinder))]GetLayoutsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Layout>()
                .Select(layout => new LayoutModel
                    {
                        Id = layout.Id,
                        Version = layout.Version,
                        CreatedBy = layout.CreatedByUser,
                        CreatedOn = layout.CreatedOn,
                        LastModifiedBy = layout.ModifiedByUser,
                        LastModifiedOn = layout.ModifiedOn,

                        Name = layout.Name,
                        LayoutPath = layout.LayoutPath,
                        PreviewUrl = layout.PreviewUrl
                    })
                .ToDataListResponse(request);

            return new GetLayoutsResponse
                       {
                           Data = listResponse
                       };
        }

        [Route("layouts")]
        public PostLayoutResponse Post(PostLayoutRequest request)
        {
            var result = layoutService.Put(new PutLayoutRequest
                {
                    Data = request.Data,
                    User = request.User
                });

            return new PostLayoutResponse { Data = result.Data };
        }
    }
}