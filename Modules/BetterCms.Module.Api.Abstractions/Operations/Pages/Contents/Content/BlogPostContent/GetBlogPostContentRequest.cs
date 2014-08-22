using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [Route("/blog-posts/content/{ContentId}", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetBlogPostContentRequest : IReturn<GetBlogPostContentResponse>
    {
        [DataMember]
        public System.Guid ContentId
        {
            get; set;
        }
    }
}