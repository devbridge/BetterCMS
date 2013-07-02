using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    public class BlogPostService : Service, IBlogPostService
    {
        public GetBlogPostResponse Get(GetBlogPostRequest request)
        {
            // TODO: need implementation
            return new GetBlogPostResponse
                       {
                           Data = new BlogPostModel
                                      {
                                          Id = request.BlogPostId
                                      }
                       };
        }
    }
}