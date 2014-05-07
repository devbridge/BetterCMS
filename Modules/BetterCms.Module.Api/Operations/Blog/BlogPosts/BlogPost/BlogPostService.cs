using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Api.Extensions.BlogPosts;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;

using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

using ITagService = BetterCms.Module.Pages.Services.ITagService;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    public class BlogPostService : Service, IBlogPostService
    {
        private readonly IRepository repository;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        private readonly IBlogPostPropertiesService propertiesService;
        
        private readonly IBlogPostContentService contentService;

        private readonly IBlogService blogService;

        private readonly IBlogSaveService blogSaveService;
        
        private readonly ISecurityService securityService;
        
        private readonly ITagService tagService;
        
        private readonly IUnitOfWork unitOfWork;

        private readonly IMasterPageService masterPageService;
        
        private readonly IUrlService urlService;
        
        private readonly IAccessControlService accessControlService;
        
        private readonly IPageService pageService;

        public BlogPostService(IBlogSaveService blogSaveService, IBlogPostPropertiesService propertiesService, IBlogPostContentService contentService,
            IBlogService blogService, IRepository repository, IMediaFileUrlResolver fileUrlResolver, ISecurityService securityService,
            ITagService tagService, IUnitOfWork unitOfWork, IMasterPageService masterPageService, IUrlService urlService,
            IAccessControlService accessControlService, IPageService pageService)
        {
            this.blogSaveService = blogSaveService;
            this.propertiesService = propertiesService;
            this.contentService = contentService;
            this.blogService = blogService;
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.securityService = securityService;
            this.tagService = tagService;
            this.unitOfWork = unitOfWork;
            this.masterPageService = masterPageService;
            this.urlService = urlService;
            this.accessControlService = accessControlService;
            this.pageService = pageService;
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

            /*Module.Blog.Models.BlogPost blogPost = null;
            PageContent pageContent = null;
            BlogPostContent content = null;

            if (!isNew)
            {
                blogPost = repository
                    .AsQueryable<Module.Blog.Models.BlogPost>(bp => bp.Id == request.BlogPostId.Value)
                    .FetchMany(bp => bp.AccessRules)
                    .FetchMany(bp => bp.PageTags)
                    .ThenFetch(pt => pt.Tag)
                    .FirstOrDefault();

                if (blogPost != null)
                {
                    if (request.Data.TechnicalInfo != null)
                    {
                        content = repository
                            .AsQueryable<BlogPostContent>(c => c.PageContents.Any(x => x.Page.Id == request.BlogPostId
                                && !x.IsDeleted && x.Id == request.Data.TechnicalInfo.PageContentId.Value
                                && !x.IsDeleted && c.Id == request.Data.TechnicalInfo.BlogPostContentId.Value))
                            .ToFuture()
                            .FirstOrDefault();

                        if (content == null)
                        {
                            const string message = "Cannot find a blog post content by specified blog post content and Id and page content Id.";
                            var logMessage = string.Format("{0}. BlogId: {1}, BlogPostContentId: {2}, PageContentId: {3}");
                            throw new ValidationException(() => message, logMessage);
                        }

                        pageContent = repository.First<PageContent>(pc => pc.Id == request.Data.TechnicalInfo.PageContentId.Value);
                    }
                    else
                    {
                        content = repository
                            .AsQueryable<BlogPostContent>(c => c.PageContents.Any(x => x.Page.Id == request.BlogPostId && !x.IsDeleted) && !c.IsDeleted)
                            .ToFuture()
                            .FirstOrDefault();

                        pageContent = repository.FirstOrDefault<PageContent>(c => c.Page.Id == request.BlogPostId && !c.IsDeleted && c.Content == content);
                    }
                }
            }

            isNew = blogPost == null;

            TODO: BEFORE ALL THE TRANSACTIONS
            // Load master pages for updating page's master path and page's children master path
            IList<Guid> newMasterIds;
            IList<Guid> oldMasterIds;
            IList<Guid> childrenPageIds;
            IList<MasterPage> existingChildrenMasterPages;
            masterPageService.PrepareForUpdateChildrenMasterPages(blogPost, request.Data.MasterPageId, out newMasterIds, out oldMasterIds, out childrenPageIds, out existingChildrenMasterPages);

            unitOfWork.BeginTransaction();

            repository.Save(blogPost);
            repository.Save(content);
            repository.Save(pageContent);

            TODO: AFTER SAVE BEFORE COMMIT: 
            masterPageService.UpdateChildrenMasterPages(existingChildrenMasterPages, oldMasterIds, newMasterIds, childrenPageIds);

            unitOfWork.Commit();
            */
        }
    }
}