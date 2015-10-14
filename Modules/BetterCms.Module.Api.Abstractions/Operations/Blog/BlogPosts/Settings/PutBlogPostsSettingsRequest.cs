using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    /// <summary>
    /// Request for blogs settings update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutBlogPostsSettingsRequest : RequestBase<SaveBlogPostsSettingsModel>
    {
    }
}