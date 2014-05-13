using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [Route("/layouts/{LayoutId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetLayoutRequest : RequestBase<GetLayoutModel>, IReturn<GetLayoutResponse>
    {
        [DataMember]
        public Guid LayoutId { get; set; }
    }
}