using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options
{
    [Route("/layouts/{LayoutId}/options", Verbs = "GET")]
    [DataContract]
    public class GetLayoutOptionsRequest : RequestBase<DataOptions>, IReturn<GetLayoutOptionsResponse>
    {
        [DataMember]
        public System.Guid LayoutId { get; set; }
    }
}
