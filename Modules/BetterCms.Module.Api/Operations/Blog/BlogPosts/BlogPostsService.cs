using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts
{
    public class BlogPostsService : Service, IBlogPostsService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public BlogPostsService(IRepository repository, IMediaFileUrlResolver fileUrlResolver)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
        }

        public GetBlogPostsResponse Get(GetBlogPostsRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Blog.Models.BlogPost>();

            if (!request.Data.IncludeArchived)
            {
                query = query.Where(b => !b.IsArchived);
            }

            if (!request.Data.IncludeUnpublished)
            {
                query = query.Where(b => b.Status == PageStatus.Published);
            }

            query = query.ApplyPageTagsFilter(request.Data);

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
                        LayoutId = blogPost.Layout != null && !blogPost.Layout.IsDeleted ? blogPost.Layout.Id : Guid.Empty,
                        CategoryId = blogPost.Category != null && !blogPost.Category.IsDeleted ? blogPost.Category.Id : (Guid?)null,
                        CategoryName = blogPost.Category != null && !blogPost.Category.IsDeleted ? blogPost.Category.Name : null,
                        AuthorId = blogPost.Author != null && !blogPost.Author.IsDeleted ? blogPost.Author.Id : (Guid?)null,
                        AuthorName = blogPost.Author != null && !blogPost.Author.IsDeleted ? blogPost.Author.Name : null,
                        MainImageId = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.Id : (Guid?)null,
                        MainImageUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicThumbnailUrl : null,
                        MainImageCaption = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.Caption : null,
                        ActivationDate = blogPost.ActivationDate,
                        ExpirationDate = blogPost.ExpirationDate,
                        IsArchived = blogPost.IsArchived
                    })
                    .ToDataListResponse(request);

            foreach (var model in listResponse.Items)
            {
                model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
                model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);
            }

            if (request.Data.IncludeTags)
            {
                LoadTags(listResponse);
            }

            return new GetBlogPostsResponse
                       {
                           Data = listResponse
                       };
        }

        private void LoadTags(DataListResponse<BlogPostModel> response)
        {
            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();

            var tags = repository
                    .AsQueryable<PageTag>(pt => pageIds.Contains(pt.Page.Id))
                    .Select(pt => new { PageId = pt.Page.Id, TagName = pt.Tag.Name })
                    .OrderBy(o => o.TagName)
                    .ToList();

            response.Items.ToList().ForEach(page => { page.Tags = tags.Where(tag => tag.PageId == page.Id).Select(tag => tag.TagName).ToList(); });
        }
    }
}