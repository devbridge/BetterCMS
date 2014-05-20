using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    [Route("/blog-settings", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetBlogPostsSettingsRequest : RequestBase<GetBlogPostsSettingsModel>, IReturn<GetBlogPostsSettingsResponse>
    {
    }
}