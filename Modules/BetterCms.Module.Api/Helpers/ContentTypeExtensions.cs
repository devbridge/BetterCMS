using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Api.Helpers
{
    public static class ContentTypeExtensions
    {
        public static string ToContentTypeString(this System.Type type)
        {
            if (type == typeof(BlogPostContent))
            {
                return Blog.Accessors.BlogPostContentAccessor.ContentWrapperType;
            }
            
            if (type == typeof(HtmlContent))
            {
                return Pages.Accessors.HtmlContentAccessor.ContentWrapperType;
            }
            
            if (type == typeof(ServerControlWidget))
            {
                return Pages.Accessors.ServerControlWidgetAccessor.ContentWrapperType;
            }
            
            if (type == typeof(HtmlContentWidget))
            {
                return Pages.Accessors.HtmlContentWidgetAccessor.ContentWrapperType;
            }

            return null;
        }
    }
}