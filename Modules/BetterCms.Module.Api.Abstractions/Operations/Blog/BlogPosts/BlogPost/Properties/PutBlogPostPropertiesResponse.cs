using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Blog post update response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutBlogPostPropertiesResponse : SaveResponseBase
    {
    }
}
