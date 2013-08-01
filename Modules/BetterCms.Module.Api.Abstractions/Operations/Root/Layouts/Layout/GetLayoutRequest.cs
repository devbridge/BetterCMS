using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [Route("/layouts/{LayoutId}", Verbs = "GET")]
    [DataContract]
    public class GetLayoutRequest : IReturn<GetLayoutResponse>
    {
        [DataMember]
        public System.Guid LayoutId { get; set; }
    }
}