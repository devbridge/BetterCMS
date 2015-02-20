using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options
{
    public class LayoutOptionsService : Service, ILayoutOptionsService
    {
        private readonly IRepository repository;

        public LayoutOptionsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetLayoutOptionsResponse Get(GetLayoutOptionsRequest request)
        {
            var results = LayoutServiceHelper.GetLayoutOptionsResponse(repository, request.LayoutId, request);

            return new GetLayoutOptionsResponse { Data = results };
        }
    }
}