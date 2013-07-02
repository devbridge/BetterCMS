using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    public class BlogPostsService : Service, IBlogPostsService
    {
        private readonly IBlogPostPropertiesService propertiesService;

        public BlogPostsService(IBlogPostPropertiesService propertiesService)
        {
            this.propertiesService = propertiesService;
        }

        public GetBlogPostsResponse Get(GetBlogPostsRequest request)
        {
            // TODO: need implementation
            return new GetBlogPostsResponse
                       {
                           Data =
                               new DataListResponse<BlogPostModel>
                                   {
                                       TotalCount = 125,
                                       Items =
                                           new List<BlogPostModel>
                                               {
                                                   new BlogPostModel(),
                                                   new BlogPostModel(),
                                                   new BlogPostModel(),
                                               }
                                   }
                       };
        }

        IBlogPostPropertiesService IBlogPostsService.Properties
        {
            get
            {
                return propertiesService;
            }
        }
    }
}