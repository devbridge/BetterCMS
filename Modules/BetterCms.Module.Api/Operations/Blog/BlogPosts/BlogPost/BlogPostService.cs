using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    public class BlogPostService : Service, IBlogPostService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        private readonly IBlogPostPropertiesService propertiesService;
        
        private readonly IBlogPostContentService contentService;

        public BlogPostService(IBlogPostPropertiesService propertiesService, IBlogPostContentService contentService,
            IRepository repository, IMediaFileUrlResolver fileUrlResolver)
        {
            this.propertiesService = propertiesService;
            this.contentService = contentService;
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
        }

        public GetBlogPostResponse Get(GetBlogPostRequest request)
        {
            var model = repository
                .AsQueryable<Module.Blog.Models.BlogPost>(blogPost => blogPost.Id == request.BlogPostId)
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
                        LayoutId = blogPost.Layout != null && !blogPost.Layout.IsDeleted ? blogPost.Layout.Id : (Guid?)null,
                        MasterPageId = blogPost.MasterPage != null && !blogPost.MasterPage.IsDeleted ? blogPost.MasterPage.Id : (Guid?)null,
                        AuthorId = blogPost.Author != null && !blogPost.Author.IsDeleted ? blogPost.Author.Id : (Guid?)null,
                        AuthorName = blogPost.Author != null && !blogPost.Author.IsDeleted ? blogPost.Author.Name : null,
                        MainImageId = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.Id : (Guid?)null,
                        MainImageUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicThumbnailUrl : null,
                        MainImageThumbnailUrl = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.PublicThumbnailUrl : null,
                        MainImageCaption = blogPost.Image != null && !blogPost.Image.IsDeleted ? blogPost.Image.Caption : null,
                        SecondaryImageId = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.Id : (Guid?)null,
                        SecondaryImageUrl = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.PublicUrl : null,
                        SecondaryImageThumbnailUrl = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.PublicThumbnailUrl : null,
                        SecondaryImageCaption = blogPost.SecondaryImage != null && !blogPost.SecondaryImage.IsDeleted ? blogPost.SecondaryImage.Caption : null,
                        FeaturedImageId = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.Id : (Guid?)null,
                        FeaturedImageUrl = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.PublicUrl : null,
                        FeaturedImageThumbnailUrl = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.PublicThumbnailUrl : null,
                        FeaturedImageCaption = blogPost.FeaturedImage != null && !blogPost.FeaturedImage.IsDeleted ? blogPost.FeaturedImage.Caption : null,
                        ActivationDate = blogPost.ActivationDate,
                        ExpirationDate = blogPost.ExpirationDate,
                        IsArchived = blogPost.IsArchived,
                        UseCanonicalUrl = blogPost.UseCanonicalUrl,
                        LanguageId = blogPost.Language != null ? blogPost.Language.Id : (Guid?)null,
                        LanguageCode = blogPost.Language != null ? blogPost.Language.Code : null,
                        LanguageGroupIdentifier = blogPost.LanguageGroupIdentifier
                    })
                .FirstOne();

            model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
            model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);
            model.MainImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnailUrl);

            model.SecondaryImageUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageUrl);
            model.SecondaryImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.SecondaryImageThumbnailUrl);

            model.FeaturedImageUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageUrl);
            model.FeaturedImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.FeaturedImageThumbnailUrl);

            LoadContentId(model);

            model.Categories = (from pagePr in repository.AsQueryable<Module.Blog.Models.BlogPost>()
                                from category in pagePr.Categories
                                where pagePr.Id == model.Id && !category.IsDeleted
                                select new CategoryModel
                                {
                                    Id = category.Category.Id,
                                    Version = category.Version,
                                    CreatedBy = category.CreatedByUser,
                                    CreatedOn = category.CreatedOn,
                                    LastModifiedBy = category.ModifiedByUser,
                                    LastModifiedOn = category.ModifiedOn,
                                    Name = category.Category.Name
                                }).ToList();

            return new GetBlogPostResponse
                       {
                           Data = model
                       };
        }

        private void LoadContentId(BlogPostModel post)
        {
            post.ContentId =
                        repository.AsQueryable<Module.Root.Models.PageContent>(pc => pc.Page.Id == post.Id && !pc.Content.IsDeleted && pc.Content is BlogPostContent)
                            .Select(pc => pc.Content.Id)
                            .FirstOrDefault();
        }

        IBlogPostPropertiesService IBlogPostService.Properties
        {
            get
            {
                return propertiesService;
            }
        }

        IBlogPostContentService IBlogPostService.Content
        {
            get
            {
                return contentService;
            }
        }
    }
}