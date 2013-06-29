using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    public class LayoutRegionsService : Service, ILayoutRegionService
    {
        public GetLayoutRegionsResponse Get(GetLayoutRegionsRequest request)
        {
            return new GetLayoutRegionsResponse
                       {
                           Data =
                               new DataListResponse<RegionModel>
                                   {
                                       Items =
                                           new List<RegionModel>
                                               {
                                                   new RegionModel(),
                                                   new RegionModel(),
                                                   new RegionModel()
                                               },
                                       TotalCount = 10
                                   }
                       };
        }
    }
}