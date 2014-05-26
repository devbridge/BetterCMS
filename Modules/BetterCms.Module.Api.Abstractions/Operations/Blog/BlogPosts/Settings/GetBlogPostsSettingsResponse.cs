using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    [DataContract]
    [Serializable]
    public class GetBlogPostsSettingsResponse : ResponseBase<BlogPostsSettingsModel>
    {
    }
}