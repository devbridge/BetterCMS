using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;

using BetterCms.Module.Api.ApiExtensions;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [RoutePrefix("bcms-api")]
    public class LayoutController : ApiController, ILayoutService
    {
        private readonly ILayoutRegionsService layoutRegionService;
        
        private readonly ILayoutOptionsService layoutOptionsService;

        private readonly IRepository repository;

        private readonly Module.Pages.Services.ILayoutService layoutService;

        public LayoutController(ILayoutRegionsService layoutRegionService, IRepository repository, ILayoutOptionsService layoutOptionsService,
            Module.Pages.Services.ILayoutService layoutService)
        {
            this.layoutRegionService = layoutRegionService;
            this.layoutOptionsService = layoutOptionsService;
            this.repository = repository;
            this.layoutService = layoutService;
        }

        [Route("layouts/{LayoutId}")]
        public GetLayoutResponse Get([ModelBinder(typeof(JsonModelBinder))]GetLayoutRequest request)
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

            var response = new GetLayoutResponse { Data = model };

            if (request.Data.IncludeOptions)
            {
                response.Options = LayoutServiceHelper.GetLayoutOptionsList(repository, request.LayoutId);
            }

            if (request.Data.IncludeRegions)
            {
                response.Regions = LayoutServiceHelper.GetLayoutRegionsList(repository, request.LayoutId);
            }

            return response;
        }

        [Route("layouts/{Id}")]
        [UrlPopulator]
        public PutLayoutResponse Put(PutLayoutRequest request)
        {
            var model = request.Data.ToServiceModel();
            if (request.Id.HasValue)
            {
                model.Id = request.Id.Value;
            }

            var result = layoutService.SaveLayout(model, false, true);

            return new PutLayoutResponse { Data = result.Id };
        }

        [Route("layouts/{Id}")]
        [UrlPopulator]
        public DeleteLayoutResponse Delete(DeleteLayoutRequest request)
        {
            var result = layoutService.DeleteLayout(request.Id, request.Data.Version);

            return new DeleteLayoutResponse { Data = result };
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