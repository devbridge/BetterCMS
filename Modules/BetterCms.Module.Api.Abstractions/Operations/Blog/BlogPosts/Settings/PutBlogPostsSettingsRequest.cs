using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    /// <summary>
    /// Request for blogs settings update or creation.
    /// </summary>
    [Route("/blog-settings", Verbs = "Put")]
    [DataContract]
    [Serializable]
    public class PutBlogPostsSettingsRequest : RequestBase<SaveBlogPostsSettingsModel>, IReturn<PutBlogPostsSettingsResponse>
    {
    }
}