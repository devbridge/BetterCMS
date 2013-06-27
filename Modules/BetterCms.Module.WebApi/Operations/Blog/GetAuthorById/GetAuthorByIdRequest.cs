using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetAuthorById
{
    [DataContract]
    public class GetAuthorByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        /// <value>
        /// The author id.
        /// </value>
        [DataMember(Order = 10, Name = "authorId")]
        public System.Guid AuthorId { get; set; }
    }
}