using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Api.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    public class LayoutRegionsService : Service, ILayoutRegionService
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
                .AsQueryable<Module.Root.Models.LayoutRegion>(lr => lr.Layout.Id == request.LayoutId)
                .Select(lr => new RegionModel
                    {
                        Id = lr.Region.Id,
                        Version = lr.Region.Version,
                        CreatedBy = lr.Region.CreatedByUser,
                        CreatedOn = lr.Region.CreatedOn,
                        LastModifiedBy = lr.Region.ModifiedByUser,
                        LastModifiedOn = lr.Region.ModifiedOn,

                        RegionIdentifier = lr.Region.RegionIdentifier
                    })
                .ToDataListResponse(request);

            return new GetLayoutRegionsResponse
                {
                    Data = listResponse
                };
        }
    }
}