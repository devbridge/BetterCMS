using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [Route("/blog-posts", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetBlogPostsRequest : RequestBase<GetBlogPostsModel>, IReturn<GetBlogPostsResponse>
    {
    }
}