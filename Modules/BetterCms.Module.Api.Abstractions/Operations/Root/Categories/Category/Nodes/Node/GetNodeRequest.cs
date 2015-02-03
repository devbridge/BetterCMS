using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    [Route("/categories/{CategoryId}/nodes/{NodeId}", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetNodeRequest : RequestBase<GetNodeModel>, IReturn<GetNodeResponse>
    {
        [DataMember]
        public Guid CategoryId { get; set; }

        [DataMember]
        public Guid NodeId { get; set; }
    }
}