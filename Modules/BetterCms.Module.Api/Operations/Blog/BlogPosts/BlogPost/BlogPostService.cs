using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Pages.Contents.Content.BlogPostContent;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;

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
        
        private readonly ISecurityService securityService;
        
        private readonly ITagService tagService;
        
        private readonly IUnitOfWork unitOfWork;

        private readonly IMasterPageService masterPageService;
        
        private readonly IUrlService urlService;
        
        private readonly IAccessControlService accessControlService;
        
        private readonly IPageService pageService;

        public BlogPostService(IBlogPostPropertiesService propertiesService, IBlogPostContentService contentService,
            IBlogService blogService, IRepository repository, IMediaFileUrlResolver fileUrlResolver, ISecurityService securityService,
            ITagService tagService, IUnitOfWork unitOfWork, IMasterPageService masterPageService, IUrlService urlService,
            IAccessControlService accessControlService, IPageService pageService)
        {
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
            Module.Blog.Models.BlogPost blogPost = null;
            PageContent pageContent = null;
            BlogPostContent content = null;

            // Validate technical info
            if (request.Data.TechnicalInfo != null)
            {
                if (!request.Data.TechnicalInfo.BlogPostContentId.HasValue || request.Data.TechnicalInfo.BlogPostContentId.Value.HasDefaultValue())
                {
                    const string message = "Blog post content ID should be specified if technical info is set.";
                    throw new ValidationException(() => message, message);
                }
                if (!request.Data.TechnicalInfo.PageContentId.HasValue || request.Data.TechnicalInfo.PageContentId.Value.HasDefaultValue())
                {
                    const string message = "Page content ID should be specified if technical info is set.";
                    throw new ValidationException(() => message, message);
                }
                if (!request.Data.TechnicalInfo.RegionId.HasValue || request.Data.TechnicalInfo.RegionId.Value.HasDefaultValue())
                {
                    const string message = "Region ID should be specified if technical info is set.";
                    throw new ValidationException(() => message, message);
                }
            }

            Guid? id = !request.BlogPostId.HasValue || request.BlogPostId.Value.HasDefaultValue() ? (Guid?)null : request.BlogPostId.Value;
            var isNew = !id.HasValue;

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

            // Validate
            if (!isNew)
            {
                if (request.Data.LayoutId == null && request.Data.MasterPageId == null)
                {
                    const string message = "Master page id or layout id should be set when updating blog post.";
                    throw new ValidationException(() => message, message);
                }

                if (string.IsNullOrWhiteSpace(request.Data.BlogPostUrl))
                {
                    const string message = "Blog post URL cannot be null when updating blog post.";
                    throw new ValidationException(() => message, message);
                }
                request.Data.BlogPostUrl = urlService.FixUrl(request.Data.BlogPostUrl);
                pageService.ValidatePageUrl(request.Data.BlogPostUrl, request.BlogPostId.Value);
            }

            if (isNew)
            {
                if (string.IsNullOrWhiteSpace(request.Data.BlogPostUrl))
                {
                    request.Data.BlogPostUrl = blogService.CreateBlogPermalink(request.Data.Title);
                }

                // Create blog post and blog post contents
                blogPost = new Module.Blog.Models.BlogPost();
                if (request.BlogPostId.HasValue)
                {
                    blogPost.Id = request.BlogPostId.Value;
                }

                content = new BlogPostContent();
                pageContent = new PageContent
                {
                    Page = blogPost,
                    Content = content
                };
                content.Name = request.Data.Title;
                if (request.Data.TechnicalInfo != null)
                {
                    pageContent.Id = request.Data.TechnicalInfo.PageContentId.Value;
                    content.Id = request.Data.TechnicalInfo.BlogPostContentId.Value;
                    pageContent.Region = repository.AsProxy<Region>(request.Data.TechnicalInfo.RegionId.Value);
                }
            }

            // Load master pages for updating page's master path and page's children master path
            IList<Guid> newMasterIds;
            IList<Guid> oldMasterIds;
            IList<Guid> childrenPageIds;
            IList<MasterPage> existingChildrenMasterPages;
            masterPageService.PrepareForUpdateChildrenMasterPages(blogPost, request.Data.MasterPageId, out newMasterIds, out oldMasterIds, out childrenPageIds, out existingChildrenMasterPages);

            unitOfWork.BeginTransaction();

            // Set master page or layout and region
            Page masterPage = null;
            if (request.Data.MasterPageId.HasValue)
            {
                // Set master page
                masterPage = repository.AsProxy<Page>(request.Data.MasterPageId.Value);
                blogPost.MasterPage = masterPage;
                blogPost.Layout = null;
            }
            else if (request.Data.LayoutId.HasValue)
            {
                // Set layout
                blogPost.Layout = repository.AsProxy<Layout>(request.Data.LayoutId.Value);
                blogPost.MasterPage = null;
            }
            else if (isNew)
            {
                Layout layout;
                Region region;
                blogService.LoadDefaultLayoutAndRegion(out layout, out masterPage, out region);
                if (layout != null)
                {
                    blogPost.Layout = layout;
                }
                else
                {
                    blogPost.MasterPage = masterPage;
                    masterPageService.SetPageMasterPages(blogPost, masterPage.Id);
                }

                if (request.Data.TechnicalInfo == null)
                {
                    pageContent.Region = region;
                }
            }

            // Set all the properties
            if (request.Data.MetaData != null)
            {
                blogPost.MetaTitle = request.Data.MetaData.MetaTitle;
                blogPost.MetaKeywords = request.Data.MetaData.MetaKeywords;
                blogPost.MetaDescription = request.Data.MetaData.MetaDescription;
            }
            if (request.Data.Version > 0)
            {
                blogPost.Version = request.Data.Version;
            }
            blogPost.ActivationDate = request.Data.ActivationDate;
            blogPost.ExpirationDate = TimeHelper.FormatEndDate(request.Data.ExpirationDate);
            blogPost.Description = request.Data.IntroText;
            blogPost.UseCanonicalUrl = request.Data.UseCanonicalUrl;
            blogPost.UseNoFollow = request.Data.UseNoFollow;
            blogPost.UseNoIndex = request.Data.UseNoIndex;
            blogPost.Author = request.Data.AuthorId.HasValue ? repository.AsProxy<Author>(request.Data.AuthorId.Value) : null;
            blogPost.Category = request.Data.CategoryId.HasValue ? repository.AsProxy<Category>(request.Data.CategoryId.Value) : null;
            blogPost.Image = request.Data.MainImageId.HasValue ? repository.AsProxy<MediaImage>(request.Data.MainImageId.Value) : null;
            blogPost.SecondaryImage = request.Data.SecondaryImageId.HasValue ? repository.AsProxy<MediaImage>(request.Data.SecondaryImageId.Value) : null;
            blogPost.FeaturedImage = request.Data.FeaturedImageId.HasValue ? repository.AsProxy<MediaImage>(request.Data.FeaturedImageId.Value) : null;
            blogPost.IsArchived = request.Data.IsArchived;
            blogPost.PageUrl = urlService.FixUrl(request.Data.BlogPostUrl);
            if (blogPost.PageUrl != null)
            {
                blogPost.PageUrlHash = request.Data.BlogPostUrl.UrlHash();
            }
            blogPost.Title = request.Data.Title;
            blogPost.Status = request.Data.IsPublished ? PageStatus.Published : PageStatus.Unpublished;
            blogPost.PublishedOn = request.Data.PublishedOn;
            
            content.Html = request.Data.HtmlContent ?? string.Empty;
            content.Status = request.Data.IsPublished ? ContentStatus.Published : ContentStatus.Draft;
            content.ActivationDate = request.Data.ActivationDate;
            content.ExpirationDate = request.Data.ExpirationDate;

            // Add default access rules if request rules are not set
            if (isNew && request.Data.AccessRules == null)
            {
                blogService.AddDefaultAccessRules(blogPost, securityService.GetCurrentPrincipal(), masterPage);
            }

            // Add access rules from the request
            if (request.Data.AccessRules != null)
            {
                blogPost.AccessRules.RemoveDuplicateEntities();
                var accessRules = request.Data.AccessRules
                    .Select(r => (IAccessRule)new AccessRule
                                              {
                                                  AccessLevel = (AccessLevel)(int)r.AccessLevel, 
                                                  Identity = r.Identity, 
                                                  IsForRole = r.IsForRole
                                              })
                        .ToList();
                accessControlService.UpdateAccessControl(blogPost, accessRules);
            }
            IList<Tag> newTags = null;
            if (request.Data.Tags != null)
            {
                tagService.SavePageTags(blogPost, request.Data.Tags, out newTags);
            }

            repository.Save(blogPost);
            repository.Save(content);
            repository.Save(pageContent);

            masterPageService.UpdateChildrenMasterPages(existingChildrenMasterPages, oldMasterIds, newMasterIds, childrenPageIds);

            unitOfWork.Commit();

            // Notify about new created tags.
            if (newTags != null)
            {
                Events.RootEvents.Instance.OnTagCreated(newTags);
            }

            // Notify about new or updated blog post.
            if (isNew)
            {
                Events.BlogEvents.Instance.OnBlogCreated(blogPost);
            }
            else
            {
                Events.BlogEvents.Instance.OnBlogUpdated(blogPost);
            }

            return new PutBlogPostResponse { Data = blogPost.Id };
        }
    }
}