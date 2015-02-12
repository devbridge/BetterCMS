using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Request to delete category node.
    /// </summary>
    [Route("/categorytrees/{CategoryTreeId}/nodes/{Id}", Verbs = "DELETE")]
    [Serializable]
    [DataContract]
    public class DeleteNodeRequest : DeleteRequestBase, IReturn<DeleteNodeResponse>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}