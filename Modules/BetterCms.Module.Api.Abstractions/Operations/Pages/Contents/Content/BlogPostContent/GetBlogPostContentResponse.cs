using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    [DataContract]
    public class GetBlogPostContentResponse : ResponseBase<BlogPostContentModel>
    {
    }
}