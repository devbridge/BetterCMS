using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/categories/{CategoryId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutCategoryRequest : RequestBase<SaveCategoryModel>, IReturn<PutCategoryResponse>
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [DataMember]
        public Guid? CategoryId { get; set; }
    }
}