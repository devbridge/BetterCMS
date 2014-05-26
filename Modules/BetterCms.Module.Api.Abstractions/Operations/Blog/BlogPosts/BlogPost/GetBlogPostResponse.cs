using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [DataContract]
    [Serializable]
    public class GetBlogPostResponse : ResponseBase<BlogPostModel>
    {
    }
}