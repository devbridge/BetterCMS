using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes.Node
{
    /// <summary>
    /// Request to save category node.
    /// </summary>
    [Route("/categories/{CategoryId}/nodes/{Id}", Verbs = "PUT")]
    [Serializable]
    [DataContract]
    public class PutNodeRequest : PutRequestBase<SaveNodeModel>, IReturn<PutNodeResponse>
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