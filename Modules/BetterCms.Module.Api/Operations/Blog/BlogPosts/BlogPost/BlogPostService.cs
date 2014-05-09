using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Services;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;

using BetterCms.Module.Blog.Services;
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

        private readonly IBlogSaveService blogSaveService;
        
        private readonly ISecurityService securityService;

        public BlogPostService(IBlogSaveService blogSaveService, IBlogPostPropertiesService propertiesService, IBlogPostContentService contentService,
            IRepository repository, IMediaFileUrlResolver fileUrlResolver, ISecurityService securityService)
        {
            this.blogSaveService = blogSaveService;
            this.propertiesService = propertiesService;
            this.contentService = contentService;
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.securityService = securityService;
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
                        IsArchived = blogPost.IsArchived,
                        UseCanonicalUrl = blogPost.UseCanonicalUrl,
                    })
                .FirstOne();

            model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
            model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);

            return new GetBlogPostResponse
                       {
                           Data = model
                       };
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

        public PutBlogPostResponse Put(PutBlogPostRequest request)
        {
            var serviceModel = request.Data.ToServiceModel();
            if (request.BlogPostId.HasValue)
            {
                serviceModel.Id = request.BlogPostId.Value;
            }

            var response = blogSaveService.SaveBlogPost(serviceModel, securityService.GetCurrentPrincipal());

            return new PutBlogPostResponse { Data = response.Id };
        }
    }
}