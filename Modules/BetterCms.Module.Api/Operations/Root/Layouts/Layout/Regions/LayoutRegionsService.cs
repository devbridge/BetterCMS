using BetterModules.Core.DataAccess;

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
            var listResponse = LayoutServiceHelper.GetLayoutRegionsResponse(repository, request.LayoutId, request);

            return new GetLayoutRegionsResponse { Data = listResponse };
        }
    }
}