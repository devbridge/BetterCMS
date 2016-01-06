using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Content
{
    public class BlogPostContentService : Service, IBlogPostContentService
    {
        private readonly IRepository repository;

        public BlogPostContentService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetBlogPostContentResponse Get(GetBlogPostContentRequest request)
        {
            var model = repository
                .AsQueryable<Module.Blog.Models.BlogPostContent>(content => content.Id == request.ContentId)
                .Select(content => new BlogPostContentModel
                    {
                        Id = content.Id,
                        Version = content.Version,
                        CreatedBy = content.CreatedByUser,
                        CreatedOn = content.CreatedOn,
                        LastModifiedBy = content.ModifiedByUser,
                        LastModifiedOn = content.ModifiedOn,

                        Name = content.Name,
                        Html = content.Html,
                        IsPublished = content.Status == ContentStatus.Published,
                        PublishedByUser = content.Status == ContentStatus.Published ? content.PublishedByUser : null,
                        PublishedOn = content.Status == ContentStatus.Published ? content.PublishedOn : null
                    })
                .FirstOne();

            return new GetBlogPostContentResponse { Data = model };
        }
    }
}