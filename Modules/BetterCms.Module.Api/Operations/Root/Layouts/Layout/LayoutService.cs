using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    public class LayoutService : Service, ILayoutService
    {
        private readonly ILayoutRegionsService layoutRegionService;
        
        private readonly ILayoutOptionsService layoutOptionsService;

        private readonly IRepository repository;

        public LayoutService(ILayoutRegionsService layoutRegionService, IRepository repository, ILayoutOptionsService layoutOptionsService)
        {
            this.layoutRegionService = layoutRegionService;
            this.layoutOptionsService = layoutOptionsService;
            this.repository = repository;
        }

        public GetLayoutResponse Get(GetLayoutRequest request)
        {
            var model = repository
                .AsQueryable<Module.Root.Models.Layout>(layout => layout.Id == request.LayoutId)
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
                .FirstOne();

            return new GetLayoutResponse
                       {
                           Data = model
                       };
        }

        ILayoutRegionsService ILayoutService.Regions
        {
            get
            {
                return layoutRegionService;
            }
        }
        
        ILayoutOptionsService ILayoutService.Options
        {
            get
            {
                return layoutOptionsService;
            }
        }
    }
}