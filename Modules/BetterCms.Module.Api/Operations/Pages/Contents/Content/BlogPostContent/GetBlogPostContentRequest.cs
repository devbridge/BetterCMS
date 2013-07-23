using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [Route("/contents/blog-post/{ContentId}", Verbs = "GET")]
    [DataContract]
    public class GetBlogPostContentRequest : RequestBase<GetBlogPostContentModel>, IReturn<GetBlogPostContentResponse>
    {
        [DataMember]
        public System.Guid ContentId
        {
            get
            {
                return Data.ContentId;
            }
            set
            {
                Data.ContentId = value;
            }
        }
    }
}