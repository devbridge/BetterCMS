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
                query = query.Where(b => b.Status == PageStatus.Published);
            }

            query = query.ApplyTagsFilter(
                request,
                tagName => { return b => b.PageTags.Any(tag => tag.Tag.Name == tagName); });

            var listResponse = query
                .Select(blogPost => new BlogPostModel
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
                        CategoryName = blogPost.Category.Name,
                        AuthorId = blogPost.Author.Id,
                        AuthorName = blogPost.Author.Name,
                        MainImageId = blogPost.Image.Id,
                        MainImageUrl = blogPost.Image.PublicUrl,
                        MainImageThumbnauilUrl = blogPost.Image.PublicThumbnailUrl,
                        MainImageCaption = blogPost.Image.Caption,
                        ActivationDate = blogPost.ActivationDate,
                        ExpirationDate = blogPost.ExpirationDate,
                        IsArchived = blogPost.IsArchived
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