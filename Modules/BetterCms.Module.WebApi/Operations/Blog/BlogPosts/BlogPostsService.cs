using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    public class BlogPostsService : Service, IBlogPostsService
    {
        private readonly IBlogPostPropertiesService propertiesService;
        
        private readonly IRepository repository;

        public BlogPostsService(IBlogPostPropertiesService propertiesService, IRepository repository)
        {
            this.propertiesService = propertiesService;
            this.repository = repository;
        }

        public GetBlogPostsResponse Get(GetBlogPostsRequest request)
        {
            request.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Blog.Models.BlogPost>();

            if (!request.IncludeArchived)
            {
                query = query.Where(b => !b.IsArchived);
            }
            
            if (!request.IncludeUnpublished)
            {
                query = query.Where(b => b.Status == PageStatus.Published 
                    && b.ActivationDate < System.DateTime.Now 
                    && (!b.ExpirationDate.HasValue || System.DateTime.Now < b.ExpirationDate.Value));
            }

            // TODO: filter by tags !!!

            var listResponse = query
                .Select(blogPost => new BlogPostModel()
                    {
                        Id = blogPost.Id,
                        Version = blogPost.Version,
                        CreatedBy = blogPost.CreatedByUser,
                        CreatedOn = blogPost.CreatedOn,
                        LastModifiedBy = blogPost.ModifiedByUser,
                        LastModifiedOn = blogPost.ModifiedOn,

                        BlogPostUrl = blogPost.PageUrl,
                        Title = blogPost.Title,
                        IntroText = blogPost.Description,
                        IsPublished = blogPost.Status == PageStatus.Published,
                        PublishedOn = blogPost.PublishedOn,
                        LayoutId = blogPost.Layout.Id,
                        CategoryId = blogPost.Category.Id,
                        AuthorId = blogPost.Author.Id,
                        MainImageId = blogPost.Image.Id,
                        ActivationDate = blogPost.ActivationDate,
                        ExpirationDate = blogPost.ExpirationDate
                    }).ToDataListResponse(request);

            return new GetBlogPostsResponse
                       {
                           Data = listResponse
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