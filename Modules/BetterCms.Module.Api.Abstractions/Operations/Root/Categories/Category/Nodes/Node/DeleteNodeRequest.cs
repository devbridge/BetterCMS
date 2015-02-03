using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Request to delete category node.
    /// </summary>
    [Route("/categories/{CategoryId}/nodes/{Id}", Verbs = "DELETE")]
    [Serializable]
    [DataContract]
    public class DeleteNodeRequest : DeleteRequestBase, IReturn<DeleteNodeResponse>
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [DataMember]
        public Guid CategoryId { get; set; }
    }
}