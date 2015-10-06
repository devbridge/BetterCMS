using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    [DataContract]
    [System.Serializable]
    public class GetAuthorRequest
    {
        [DataMember]
        public System.Guid AuthorId { get; set; }
    }
}