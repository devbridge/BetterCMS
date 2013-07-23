using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [Route("/layouts/{LayoutId}/regions", Verbs = "GET")]
    [DataContract]
    public class GetLayoutRegionsRequest : GetLayoutRegionsModel, IReturn<GetLayoutRegionsResponse>
    {
    }
}