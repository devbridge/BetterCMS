using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [Route("/contents/blog-post/{ContentId}", Verbs = "GET")]
    [DataContract]
    public class GetBlogPostContentRequest : RequestBase<GetBlogPostContentModel>, IReturn<GetBlogPostContentResponse>
    {
    }
}