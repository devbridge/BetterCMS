using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    [DataContract]
    public class GetAuthorModel
    {
        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        /// <value>
        /// The author id.
        /// </value>
        [DataMember]
        public System.Guid AuthorId { get; set; }
    }
}