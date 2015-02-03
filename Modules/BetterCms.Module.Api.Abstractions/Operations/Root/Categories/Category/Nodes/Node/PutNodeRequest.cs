using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Request to save category node.
    /// </summary>
    [Route("/categorytrees/{CategoryTreeId}/nodes/{Id}", Verbs = "PUT")]
    [Serializable]
    [DataContract]
    public class PutNodeRequest : PutRequestBase<SaveNodeModel>, IReturn<PutNodeResponse>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}