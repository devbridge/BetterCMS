using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    /// <summary>
    /// Blog post creation response.
    /// </summary>
    [DataContract]
    public class PostBlogPostResponse : ResponseBase<Guid?>
    {
    }
}
