using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [Route("/contents/blog-post/{ContentId}", Verbs = "GET")]
    [DataContract]
    public class GetBlogPostContentRequest : IReturn<GetBlogPostContentResponse>
    {
        [DataMember]
        public System.Guid ContentId
        {
            get; set;
        }
    }
}