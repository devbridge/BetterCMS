using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    [Route("/categorytrees/{CategoryTreeId}/nodes/{NodeId}", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetNodeRequest : RequestBase<GetNodeModel>, IReturn<GetNodeResponse>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }

        [DataMember]
        public Guid NodeId { get; set; }
    }
}