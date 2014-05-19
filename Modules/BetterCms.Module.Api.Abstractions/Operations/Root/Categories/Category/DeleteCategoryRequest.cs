using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request for category update or creation.
    /// </summary>
    [Route("/categories/{CategoryId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteCategoryRequest : DeleteRequestBase, IReturn<DeleteCategoryResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid CategoryId { get; set; }
    }
}