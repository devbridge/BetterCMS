using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    public class BlogPostPropertiesService : Service, IBlogPostPropertiesService
    {
        public GetBlogPostPropertiesResponse Get(GetBlogPostPropertiesRequest request)
        {
            // TODO: need implementation
            return new GetBlogPostPropertiesResponse
                       {
                           Data = new BlogPostPropertiesModel
                                      {
                                          Id = request.BlogPostId
                                      }
                       };
        }
    }
}