using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    public class LayoutsService : Service, ILayoutsService
    {
        private readonly IRepository repository;

        public LayoutsService(IRepository repository)
        {
            this.repository = repository;
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
    }
}