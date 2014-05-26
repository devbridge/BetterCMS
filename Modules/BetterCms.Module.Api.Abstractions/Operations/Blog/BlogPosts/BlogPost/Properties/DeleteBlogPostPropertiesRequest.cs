using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Blog post delete request for REST.
    /// </summary>
    [Route("/blog-post-properties/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteBlogPostPropertiesRequest : DeleteRequestBase, IReturn<DeleteBlogPostPropertiesResponse>
    {
    }
}