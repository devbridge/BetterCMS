using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    [DataContract]
    [Serializable]
    public class GetBlogPostsResponse : ListResponseBase<BlogPostModel>
    {
    }
}