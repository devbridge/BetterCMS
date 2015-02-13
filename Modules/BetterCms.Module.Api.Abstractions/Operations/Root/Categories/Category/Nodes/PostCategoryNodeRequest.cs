using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Request for page creation.
    /// </summary>
    [Route("/categorytrees/{CategoryTreeId}/nodes/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostCategoryNodeRequest : RequestBase<SaveNodeModel>, IReturn<PostCategoryNodeResponse>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}
