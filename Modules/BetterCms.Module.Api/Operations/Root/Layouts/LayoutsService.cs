using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    public class LayoutsService : Service, ILayoutsService
    {
        private readonly IRepository repository;
        
        private readonly ILayoutService layoutService;

        public LayoutsService(IRepository repository, ILayoutService layoutService)
        {
            this.repository = repository;
            this.layoutService = layoutService;
        }
        
        public GetLayoutsResponse Get(GetLayoutsRequest request)
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