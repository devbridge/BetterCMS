using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Blog post creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostBlogPostPropertiesResponse : SaveResponseBase
    {
    }
}
