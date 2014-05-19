using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/authors/{AuthorId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutAuthorRequest : RequestBase<SaveAuthorModel>, IReturn<PutAuthorResponse>
    {
        /// <summary>
        /// Gets or sets the author identifier.
        /// </summary>
        /// <value>
        /// The author identifier.
        /// </value>
        [DataMember]
        public Guid? AuthorId { get; set; }
    }
}