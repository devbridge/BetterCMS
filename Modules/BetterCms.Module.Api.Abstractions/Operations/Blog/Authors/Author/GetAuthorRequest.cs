using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    [Route("/authors/{AuthorId}", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetAuthorRequest : IReturn<GetAuthorResponse>
    {
        [DataMember]
        public System.Guid AuthorId { get; set; }
    }
}