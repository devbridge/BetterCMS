using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    public class BlogPostService : Service, IBlogPostService
    {
        private readonly IRepository repository;

        public BlogPostService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetBlogPostResponse Get(GetBlogPostRequest request)
        {
            var model = repository
                .AsQueryable<Module.Blog.Models.BlogPost>(blogPost => blogPost.Id == request.Data.BlogPostId)
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
                    })
                .FirstOne();

            return new GetBlogPostResponse
                       {
                           Data = model
                       };
        }
    }
}