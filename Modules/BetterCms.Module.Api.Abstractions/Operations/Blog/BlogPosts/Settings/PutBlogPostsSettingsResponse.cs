using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    /// <summary>
    /// Settings creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutBlogPostsSettingsResponse : ResponseBase<bool>
    {
    }
}