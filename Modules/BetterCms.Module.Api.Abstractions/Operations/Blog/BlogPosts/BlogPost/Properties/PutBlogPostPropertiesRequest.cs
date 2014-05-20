using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Request for blog post update.
    /// </summary>
    [Route("/blog-post-properties/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutBlogPostPropertiesRequest : PutRequestBase<SaveBlogPostPropertiesModel>, IReturn<PutBlogPostPropertiesResponse>
    {
    }
}
