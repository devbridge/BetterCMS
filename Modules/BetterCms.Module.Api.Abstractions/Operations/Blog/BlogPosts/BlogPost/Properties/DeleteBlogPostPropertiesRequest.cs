using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Blog post delete request for REST.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteBlogPostPropertiesRequest : DeleteRequestBase
    {
    }
}