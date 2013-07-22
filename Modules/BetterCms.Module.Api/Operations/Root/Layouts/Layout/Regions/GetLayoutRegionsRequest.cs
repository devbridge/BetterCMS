using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [Route("/layouts/{LayoutId}/regions", Verbs = "GET")]
    public class GetLayoutRegionsRequest : GetLayoutRegionsModel, IReturn<GetLayoutRegionsResponse>
    {
    }
}