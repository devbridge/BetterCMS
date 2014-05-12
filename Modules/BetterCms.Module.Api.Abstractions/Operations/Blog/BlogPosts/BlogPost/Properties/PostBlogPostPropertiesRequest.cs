using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Request for blog post creation.
    /// </summary>
    [Route("/blog-post-properties/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostBlogPostPropertiesRequest : RequestBase<SaveBlogPostPropertiesModel>, IReturn<PostBlogPostPropertiesResponse>
    {
    }
}
