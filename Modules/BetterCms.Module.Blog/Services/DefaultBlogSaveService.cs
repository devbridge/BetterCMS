using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

using RootOptionService = BetterCms.Module.Root.Services.IOptionService;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogSaveService : DefaultBlogService, IBlogSaveService
    {        
        public DefaultBlogSaveService(ICmsConfiguration configuration, IUrlService urlService, IRepository repository, IOptionService blogOptionService, IAccessControlService accessControlService, ISecurityService securityService, IContentService contentService, ITagService tagService, IPageService pageService, IRedirectService redirectService, IMasterPageService masterPageService, IUnitOfWork unitOfWork, RootOptionService optionService, ICategoryService categoryService)
            : base(configuration, urlService, repository, blogOptionService, accessControlService, securityService, contentService, tagService, pageService, redirectService, masterPageService, unitOfWork, optionService, categoryService)
        {
        }

        protected override void PrepareForUpdateChildrenMasterPages(bool isNew, BlogPost entity, BlogPostViewModel model, out IList<Guid> newMasterIds,
            out IList<Guid> oldMasterIds, out IList<Guid> childrenPageIds, out IList<MasterPage> existingChildrenMasterPages)
        {
            var modelExt = model as BlogPostViewModelExtender;
            if (!isNew && modelExt != null)
            {
                masterPageService.PrepareForUpdateChildrenMasterPages(entity, modelExt.MasterPageId, 
                    out newMasterIds, out oldMasterIds, out childrenPageIds, out existingChildrenMasterPages);
            }
            else
            {
                newMasterIds = null;
                oldMasterIds = null;
                childrenPageIds = null;
                existingChildrenMasterPages = null; 
            }
        }

        protected override void GetBlogPostAndContentEntities(BlogPostViewModel model, IPrincipal principal, string[] roles, 
            ref bool isNew, out BlogPostContent content, out PageContent pageContent , out BlogPost blogPost)
        {
            var modelExt = model as BlogPostViewModelExtender;
            if (!isNew && modelExt != null)
            {
                content = null;
                pageContent = null;

                blogPost = repository
                    .AsQueryable<BlogPost>(bp => bp.Id == model.Id)
                    .FetchMany(bp => bp.AccessRules)
                    .FetchMany(bp => bp.PageTags)
                    .ThenFetch(pt => pt.Tag)
                    .ToList()
                    .FirstOrDefault();

                if (blogPost != null)
                {
                    if (configuration.Security.AccessControlEnabled)
                    {
                        accessControlService.DemandAccess(blogPost, principal, AccessLevel.ReadWrite, roles);
                    }

                    if (modelExt.PageContentId.HasValue)
                    {
                        content = repository
                            .AsQueryable<BlogPostContent>(c => c.PageContents.Any(x => x.Page.Id == model.Id
                                && !x.IsDeleted && x.Id == modelExt.PageContentId.Value
                                && !x.IsDeleted && c.Id == model.ContentId))
                            .ToFuture()
                            .FirstOrDefault();

                        if (content == null)
                        {
                            const string message = "Cannot find a blog post content by specified blog post content and Id and page content Id.";
                            var logMessage = string.Format("{0} BlogId: {1}, BlogPostContentId: {2}, PageContentId: {3}", 
                                message, model.Id, model.ContentId, modelExt.PageContentId);
                            throw new ValidationException(() => message, logMessage);
                        }

                        pageContent = repository.First<PageContent>(pc => pc.Id == modelExt.PageContentId.Value);
                    }
                    else
                    {
                        content = repository
                            .AsQueryable<BlogPostContent>(c => c.PageContents.Any(x => x.Page.Id == model.Id && !x.IsDeleted) && !c.IsDeleted)
                            .ToFuture()
                            .FirstOrDefault();

                        if (content != null)
                        {
                            var contentRef = content;
                            pageContent = repository.FirstOrDefault<PageContent>(c => c.Page.Id == model.Id && !c.IsDeleted && c.Content == contentRef);
                        }
                    }
                }

                isNew = blogPost == null;
                if (isNew)
                {
                    blogPost = new BlogPost();
                    pageContent = new PageContent { Page = blogPost };
                }
            }
            else
            {
                base.GetBlogPostAndContentEntities(model, principal, roles, ref isNew, out content, out pageContent, out blogPost);
            }
        }

        protected override void ValidateData(bool isNew, BlogPostViewModel model)
        {
            base.ValidateData(isNew, model);

            var modelExt = model as BlogPostViewModelExtender;
            if (modelExt != null)
            {
                // Validate technical info: if at least one of techincal fields are not null and at least one is null, throw an exception
                if (!model.ContentId.HasDefaultValue() || modelExt.PageContentId.HasValue || modelExt.RegionId.HasValue)
                {
                    if (model.ContentId.HasDefaultValue())
                    {
                        const string message = "Blog post content ID should be specified if technical info is set.";
                        throw new ValidationException(() => message, message);
                    }
                    if (!modelExt.PageContentId.HasValue || modelExt.PageContentId.Value.HasDefaultValue())
                    {
                        const string message = "Page content ID should be specified if technical info is set.";
                        throw new ValidationException(() => message, message);
                    }
                    if (!modelExt.RegionId.HasValue || modelExt.RegionId.Value.HasDefaultValue())
                    {
                        const string message = "Region ID should be specified if technical info is set.";
                        throw new ValidationException(() => message, message);
                    }
                }

                // Validate
                if (!isNew)
                {
                    if (!modelExt.LayoutId.HasValue && !modelExt.MasterPageId.HasValue)
                    {
                        const string message = "Master page id or layout id should be set when updating blog post.";
                        throw new ValidationException(() => message, message);
                    }

                    if (string.IsNullOrWhiteSpace(model.BlogUrl))
                    {
                        const string message = "Blog post URL cannot be null when updating blog post.";
                        throw new ValidationException(() => message, message);
                    }
                }
            }
        }

        protected override BlogPostContent SaveContentWithStatusUpdate(bool isNew, BlogPostContent newContent, BlogPostViewModel model, IPrincipal principal)
        {
            var modelExt = model as BlogPostViewModelExtender;
            if (isNew && modelExt != null && !modelExt.ContentId.HasDefaultValue())
            {
                contentService.UpdateDynamicContainer(newContent);
                if (model.DesirableStatus == ContentStatus.Published)
                {
                    newContent.PublishedOn = modelExt.PublishedOn ?? DateTime.Now;
                    newContent.PublishedByUser = principal.Identity.Name;
                    // TODO: pass published by user newContent.PublishedByUser = !string.IsNullOrEmpty(request.Data.PublishedByUser) ? request.Data.PublishedByUser : securityService.CurrentPrincipalName;
                }

                newContent.Status = model.DesirableStatus;
                newContent.Id = modelExt.ContentId;
                repository.Save(newContent);

                return newContent;
            }

            return base.SaveContentWithStatusUpdate(isNew, newContent, model, principal);
        }

        protected override void MapExtraProperties(bool isNew, BlogPost entity, BlogPostContent content, PageContent pageContent, BlogPostViewModel model, IPrincipal principal)
        {
            var currentVersion = entity.Version;
            base.MapExtraProperties(isNew, entity, content, pageContent, model, principal);

            var modelExt = model as BlogPostViewModelExtender;
            if (modelExt != null)
            {
                // Restore version if not set from the extended model
                if (model.Version <= 0)
                {
                    entity.Version = currentVersion;
                }

                entity.SecondaryImage = modelExt.SecondaryImageId.HasValue ? repository.AsProxy<MediaImage>(modelExt.SecondaryImageId.Value) : null;
                entity.FeaturedImage = modelExt.FeaturedImageId.HasValue ? repository.AsProxy<MediaImage>(modelExt.FeaturedImageId.Value) : null;
                entity.IsArchived = modelExt.IsArchived;
                entity.UseNoFollow = modelExt.UseNoFollow;
                entity.UseNoIndex = modelExt.UseNoIndex;
                entity.MetaKeywords = modelExt.MetaKeywords;
                entity.MetaDescription = modelExt.MetaDescription;

                if (modelExt.UpdateLanguage)
                {
                    entity.Language = modelExt.LanguageId.HasValue ? repository.AsProxy<Language>(modelExt.LanguageId.Value) : null;
                    entity.LanguageGroupIdentifier = modelExt.LanguageGroupIdentifier;
                }

                // If creating new and content / page content / region ids are set, enforce them to be set explicitly
                if (isNew && !model.ContentId.HasDefaultValue() && modelExt.PageContentId.HasValue && modelExt.RegionId.HasValue)
                {
                    pageContent.Id = modelExt.PageContentId.Value;
                    pageContent.Region = repository.AsProxy<Region>(modelExt.RegionId.Value);
                }

                // Set blog post Id, if it's set
                if (isNew && !model.Id.HasDefaultValue())
                {
                    entity.Id = model.Id;
                }

                // PublishedOn
                if (isNew && entity.Status == PageStatus.Published && modelExt.PublishedOn.HasValue)
                {
                    entity.PublishedOn = modelExt.PublishedOn.Value;
                }

                // Set layout / master page
                if (modelExt.MasterPageId.HasValue)
                {
                    entity.Layout = null;
                    if (isNew)
                    {
                        entity.MasterPage = repository
                            .AsQueryable<Page>(p => p.Id == modelExt.MasterPageId.Value)
                            .FetchMany(p => p.AccessRules)
                            .ToList()
                            .FirstOne();

                        if (modelExt.AccessRules == null)
                        {
                            AddDefaultAccessRules(entity, principal, entity.MasterPage);
                        }
                        masterPageService.SetPageMasterPages(entity, entity.MasterPage.Id);
                    }
                    else
                    {
                        entity.MasterPage = repository.AsProxy<Page>(modelExt.MasterPageId.Value);
                    }
                }
                else if (modelExt.LayoutId.HasValue)
                {
                    entity.Layout = repository.AsProxy<Layout>(modelExt.LayoutId.Value);
                    entity.MasterPage = null;
                    if (isNew && modelExt.AccessRules == null)
                    {
                        AddDefaultAccessRules(entity, principal, null);
                    }
                }

                // Add access rules from the request
                if (modelExt.AccessRules != null)
                {
                    if (entity.AccessRules == null)
                    {
                        entity.AccessRules = new List<AccessRule>();
                    }
                    else
                    {
                        entity.AccessRules.RemoveDuplicateEntities();
                    }
                    var accessRules = modelExt.AccessRules
                        .Select(r => (IAccessRule)new AccessRule
                        {
                            AccessLevel = (AccessLevel)(int)r.AccessLevel,
                            Identity = r.Identity,
                            IsForRole = r.IsForRole
                        }).ToList();

                    accessControlService.UpdateAccessControl(entity, accessRules);
                }
            }
        }

        protected override IList<Tag> SaveTags(BlogPost blogPost, BlogPostViewModel request)
        {
            if (request.Tags != null)
            {
                return base.SaveTags(blogPost, request);
            }

            return null;
        }
    }
}