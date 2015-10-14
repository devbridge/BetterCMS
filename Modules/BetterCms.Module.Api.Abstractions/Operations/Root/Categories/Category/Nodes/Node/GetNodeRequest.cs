using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    [Serializable]
    [DataContract]
    public class GetNodeRequest : RequestBase<GetNodeModel>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }

        [DataMember]
        public Guid NodeId { get; set; }
    }
}