using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent
{
    public class BlogPostContentService : Service, IBlogPostContentService
    {
        public GetBlogPostContentResponse Get(GetBlogPostContentRequest request)
        {
            // TODO: need implementation
            return new GetBlogPostContentResponse
                       {
                           Data = new BlogPostContentModel
                                      {
                                          Id = request.ContentId
                                      }
                       };
        }
    }
}