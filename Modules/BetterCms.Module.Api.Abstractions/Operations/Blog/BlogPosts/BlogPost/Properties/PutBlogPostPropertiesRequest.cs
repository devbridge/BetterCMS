using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;


namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    /// <summary>
    /// Request for blog post update.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutBlogPostPropertiesRequest : PutRequestBase<SaveBlogPostPropertiesModel>
    {
    }
}
