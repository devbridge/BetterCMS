using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Request to get category data.
    /// </summary>
    [Route("/categories/{CategoryId}/nodes/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetCategoryNodesRequest : RequestBase<DataOptions>, IReturn<GetCategoryNodesResponse>
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