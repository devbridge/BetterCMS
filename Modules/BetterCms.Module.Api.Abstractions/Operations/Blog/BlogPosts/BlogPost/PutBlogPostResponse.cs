using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    /// <summary>
    /// Blog post update response.
    /// </summary>
    [DataContract]
    public class PutBlogPostResponse : ResponseBase<Guid?>
    {
    }
}
