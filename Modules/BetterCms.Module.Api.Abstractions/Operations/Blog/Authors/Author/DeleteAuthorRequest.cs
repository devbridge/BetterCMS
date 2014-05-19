using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Request for author update or creation.
    /// </summary>
    [Route("/authors/{AuthorId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteAuthorRequest : DeleteRequestBase, IReturn<DeleteAuthorResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid AuthorId { get; set; }
    }
}