using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    public class LayoutRegionsService : Service, ILayoutRegionsService
    {
        private readonly IRepository repository;

        public LayoutRegionsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetLayoutRegionsResponse Get(GetLayoutRegionsRequest request)
        {
            request.Data.SetDefaultOrder("RegionIdentifier");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.LayoutRegion>(lr => lr.Layout.Id == request.LayoutId && !lr.Layout.IsDeleted && lr.Region != null && !lr.Region.IsDeleted)
                .Select(lr => new RegionModel
                    {
                        Id = lr.Region.Id,
                        Version = lr.Region.Version,
                        CreatedBy = lr.CreatedByUser,
                        CreatedOn = lr.CreatedOn,
                        LastModifiedBy = lr.ModifiedByUser,
                        LastModifiedOn = lr.ModifiedOn,

                        RegionIdentifier = lr.Region.RegionIdentifier,
                        Description = lr.Description
                    })
                .ToDataListResponse(request);

            return new GetLayoutRegionsResponse
                {
                    Data = listResponse
                };
        }
    }
}