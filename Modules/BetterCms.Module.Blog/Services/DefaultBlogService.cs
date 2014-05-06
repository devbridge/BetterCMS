using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;

using BetterCms.Module.MediaManager.Models;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;

using NHibernate.Criterion;
using NHibernate.Linq;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogService : IBlogService
    {
        /// <summary>
        /// The blog post region identifier.
        /// </summary>
        private const string RegionIdentifier = BlogModuleConstants.BlogPostMainContentRegionIdentifier;

        private readonly ICmsConfiguration configuration;
        private readonly IUrlService urlService;
        private readonly IRepository repository;
        private readonly IOptionService optionService;
        private readonly IAccessControlService accessControlService;
        private readonly ISecurityService securityService;
        private readonly ICmsConfiguration cmsConfiguration;
        private readonly IContentService contentService;
        private readonly IPageService pageService;
        private readonly IRedirectService redirectService;
        private readonly IMasterPageService masterPageService;
        private readonly ITagService tagService;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlogService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="contentService">The content service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="masterPageService">The master page service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultBlogService(ICmsConfiguration configuration, IUrlService urlService, IRepository repository,
            IOptionService optionService, IAccessControlService accessControlService, ISecurityService securityService,
            ICmsConfiguration cmsConfiguration, IContentService contentService, ITagService tagService,
            IPageService pageService, IRedirectService redirectService, IMasterPageService masterPageService,
            IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.urlService = urlService;
            this.repository = repository;
            this.optionService = optionService;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
            this.cmsConfiguration = cmsConfiguration;
            this.contentService = contentService;
            this.pageService = pageService;
            this.redirectService = redirectService;
            this.masterPageService = masterPageService;
            this.tagService = tagService;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Creates the blog URL from the given blog title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="unsavedUrls">The unsaved urls.</param>
        /// <returns>
        /// Created blog URL
        /// </returns>
        public string CreateBlogPermalink(string title, List<string> unsavedUrls = null)
        {
            var url = title.Transliterate(true);
            url = urlService.AddPageUrlPostfix(url, configuration.ArticleUrlPattern, unsavedUrls);

            return url;
        }

        /// <summary>
        /// Saves the blog post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>
        /// Saved blog post entity
        /// </returns>
        public BlogPost SaveBlogPost(BlogPostViewModel request, IPrincipal principal)
        {
            string[] roles;
            if (request.DesirableStatus == ContentStatus.Published)
            {
                accessControlService.DemandAccess(principal, RootModuleConstants.UserRoles.PublishContent);
                roles = new[] { RootModuleConstants.UserRoles.PublishContent };
            }
            else
            {
                accessControlService.DemandAccess(principal, RootModuleConstants.UserRoles.EditContent);
                roles = new[] { RootModuleConstants.UserRoles.EditContent };
            }

            Layout layout;
            Page masterPage;
            Region region;
            LoadDefaultLayoutAndRegion(out layout, out masterPage, out region);

            if (masterPage != null)
            {
                var level = accessControlService.GetAccessLevel(masterPage, principal);
                if (level < AccessLevel.Read)
                {
                    var message = BlogGlobalization.SaveBlogPost_FailedToSave_InaccessibleMasterPage;
                    const string logMessage = "Failed to save blog post. Selected master page for page layout is inaccessible.";
                    throw new ValidationException(() => message, logMessage);
                }
            }

            var isNew = request.Id.HasDefaultValue();
            var userCanEdit = securityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent);

            // UnitOfWork.BeginTransaction(); // NOTE: this causes concurrent data exception.

            BlogPost blogPost;
            BlogPostContent content = null;
            PageContent pageContent = null;
            Redirect redirectCreated = null;

            // Loading blog post and it's content, or creating new, if such not exists
            if (!isNew)
            {
                var blogPostFuture = repository
                    .AsQueryable<BlogPost>(b => b.Id == request.Id)
                    .ToFuture();

                content = repository
                    .AsQueryable<BlogPostContent>(c => c.PageContents.Any(x => x.Page.Id == request.Id && !x.IsDeleted))
                    .ToFuture()
                    .FirstOrDefault();

                blogPost = blogPostFuture.FirstOne();

                if (cmsConfiguration.Security.AccessControlEnabled)
                {
                    accessControlService.DemandAccess(blogPost, principal, AccessLevel.ReadWrite, roles);
                }

                if (content != null)
                {
                    // Check if user has confirmed the deletion of content
                    if (!request.IsUserConfirmed && blogPost.IsMasterPage)
                    {
                        var hasAnyChildren = contentService.CheckIfContentHasDeletingChildren(blogPost.Id, content.Id, request.Content);
                        if (hasAnyChildren)
                        {
                            var message = PagesGlobalization.SaveContent_ContentHasChildrenContents_RegionDeleteConfirmationMessage;
                            var logMessage = string.Format("User is trying to delete content regions which has children contents. Confirmation is required. ContentId: {0}, PageId: {1}",
                                    content.Id, blogPost.Id);
                            throw new ConfirmationRequestException(() => message, logMessage);
                        }
                    }

                    pageContent = repository.FirstOrDefault<PageContent>(c => c.Page == blogPost && !c.IsDeleted && c.Content == content);
                }

                if (userCanEdit && !string.Equals(blogPost.PageUrl, request.BlogUrl) && request.BlogUrl != null)
                {
                    request.BlogUrl = urlService.FixUrl(request.BlogUrl);
                    pageService.ValidatePageUrl(request.BlogUrl, request.Id);
                    if (request.RedirectFromOldUrl)
                    {
                        var redirect = redirectService.CreateRedirectEntity(blogPost.PageUrl, request.BlogUrl);
                        if (redirect != null)
                        {
                            repository.Save(redirect);
                            redirectCreated = redirect;
                        }
                    }

                    blogPost.PageUrl = urlService.FixUrl(request.BlogUrl);
                }
            }
            else
            {
                blogPost = new BlogPost();
            }

            if (pageContent == null)
            {
                pageContent = new PageContent { Region = region, Page = blogPost };
            }

            // Push to change modified data each time.
            blogPost.ModifiedOn = DateTime.Now;
            blogPost.Version = request.Version;

            if (userCanEdit)
            {
                blogPost.Title = request.Title;
                blogPost.Description = request.IntroText;
                blogPost.Author = request.AuthorId.HasValue ? repository.AsProxy<Author>(request.AuthorId.Value) : null;
                blogPost.Category = request.CategoryId.HasValue ? repository.AsProxy<Category>(request.CategoryId.Value) : null;
                blogPost.Image = (request.Image != null && request.Image.ImageId.HasValue) ? repository.AsProxy<MediaImage>(request.Image.ImageId.Value) : null;
                if (isNew || request.DesirableStatus == ContentStatus.Published)
                {
                    blogPost.ActivationDate = request.LiveFromDate;
                    blogPost.ExpirationDate = TimeHelper.FormatEndDate(request.LiveToDate);
                }
            }

            if (isNew)
            {
                if (!string.IsNullOrWhiteSpace(request.BlogUrl))
                {
                    blogPost.PageUrl = urlService.FixUrl(request.BlogUrl);
                    pageService.ValidatePageUrl(blogPost.PageUrl);
                }
                else
                {
                    blogPost.PageUrl = CreateBlogPermalink(request.Title);
                }

                blogPost.MetaTitle = request.MetaTitle ?? request.Title;
                if (masterPage != null)
                {
                    blogPost.MasterPage = masterPage;
                    masterPageService.SetPageMasterPages(blogPost, masterPage.Id);
                }
                else
                {
                    blogPost.Layout = layout;
                }
                UpdateStatus(blogPost, request.DesirableStatus);
                AddDefaultAccessRules(blogPost, principal, masterPage);
            }
            else if (request.DesirableStatus == ContentStatus.Published
                || blogPost.Status == PageStatus.Preview)
            {
                // Update only if publishing or current status is preview.
                // Else do not change, because it may change from published to draft status 
                UpdateStatus(blogPost, request.DesirableStatus);
            }

            // Create content.
            var newContent = new BlogPostContent
            {
                Id = content != null ? content.Id : Guid.Empty,
                Name = request.Title,
                Html = request.Content ?? string.Empty,
                EditInSourceMode = request.EditInSourceMode,
                ActivationDate = request.LiveFromDate,
                ExpirationDate = TimeHelper.FormatEndDate(request.LiveToDate)
            };

            // Preserve content if user is not authorized to change it.
            if (!userCanEdit)
            {
                if (content == null)
                {
                    throw new SecurityException("Forbidden: Access is denied."); // User has no rights to create new content.
                }

                var contentToPublish = (BlogPostContent)(content.History != null
                    ? content.History.FirstOrDefault(c => c.Status == ContentStatus.Draft) ?? content
                    : content);

                newContent.Name = contentToPublish.Name;
                newContent.Html = contentToPublish.Html;
            }

            content = (BlogPostContent)contentService.SaveContentWithStatusUpdate(newContent, request.DesirableStatus);
            pageContent.Content = content;

            blogPost.PageUrlHash = blogPost.PageUrl.UrlHash();
            blogPost.UseCanonicalUrl = request.UseCanonicalUrl;

            repository.Save(blogPost);
            repository.Save(content);
            repository.Save(pageContent);

            pageContent.Content = content;
            blogPost.PageContents = new [] {pageContent};

            IList<Tag> newTags = null;
            if (userCanEdit)
            {
                tagService.SavePageTags(blogPost, request.Tags, out newTags);
            }

            // Commit
            unitOfWork.Commit();

            // Notify about new created tags.
            Events.RootEvents.Instance.OnTagCreated(newTags);

            // Notify about new or updated blog post.
            if (isNew)
            {
                Events.BlogEvents.Instance.OnBlogCreated(blogPost);
            }
            else
            {
                Events.BlogEvents.Instance.OnBlogUpdated(blogPost);
            }

            // Notify about redirect creation.
            if (redirectCreated != null)
            {
                Events.PageEvents.Instance.OnRedirectCreated(redirectCreated);
            }

            return blogPost;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="blogPost">The blog post.</param>
        /// <param name="desirableStatus">The desirable status.</param>
        /// <exception cref="CmsException">If <c>desirableStatus</c> is not supported.</exception>
        private void UpdateStatus(BlogPost blogPost, ContentStatus desirableStatus)
        {
            switch (desirableStatus)
            {
                case ContentStatus.Published:
                    blogPost.Status = PageStatus.Published;
                    blogPost.PublishedOn = DateTime.Now;
                    break;
                case ContentStatus.Draft:
                    blogPost.Status = PageStatus.Unpublished;
                    break;
                case ContentStatus.Preview:
                    blogPost.Status = PageStatus.Preview;
                    break;
                default:
                    throw new CmsException(string.Format("Blog post does not support status: {0}.", desirableStatus));
            }
        }

        /// <summary>
        /// Loads the layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="masterPage">The master page.</param>
        /// <param name="region">The region.</param>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">If layout was not found.</exception>
        public void LoadDefaultLayoutAndRegion(out Layout layout, out Page masterPage, out Region region)
        {
            var option = optionService.GetDefaultOption();

            layout = option != null ? option.DefaultLayout : null;
            masterPage = option != null ? option.DefaultMasterPage : null;
            if (layout == null && masterPage == null)
            {
                layout = GetFirstCompatibleLayout();
            }

            if (layout == null && masterPage == null)
            {
                var message = BlogGlobalization.SaveBlogPost_LayoutNotFound_Message;
                const string logMessage = "No compatible layouts found for blog post.";
                throw new ValidationException(() => message, logMessage);
            }

            Guid regionId;
            if (layout != null)
            {
                regionId = layout.LayoutRegions.Count(layoutRegion => !layoutRegion.IsDeleted && !layoutRegion.Region.IsDeleted) == 1
                                   ? layout.LayoutRegions.First(layoutRegion => !layoutRegion.IsDeleted).Region.Id
                                   : layout.LayoutRegions.Where(
                                       layoutRegion =>
                                       !layoutRegion.IsDeleted && !layoutRegion.Region.IsDeleted && layoutRegion.Region.RegionIdentifier == RegionIdentifier)
                                           .Select(layoutRegion => layoutRegion.Region.Id)
                                           .FirstOrDefault();
            }
            else
            {
                var masterPageRef = masterPage;
                regionId = repository.AsQueryable<PageContent>()
                          .Where(pageContent => pageContent.Page == masterPageRef)
                          .SelectMany(pageContent => pageContent.Content.ContentRegions)
                          .Select(contentRegion => contentRegion.Region.Id)
                          .FirstOrDefault();
            }

            if (regionId.HasDefaultValue())
            {
                var message = string.Format(BlogGlobalization.SaveBlogPost_RegionNotFound_Message, RegionIdentifier);
                var logMessage = string.Format("Region {0} for rendering blog post content not found.", RegionIdentifier);
                throw new ValidationException(() => message, logMessage);
            }

            region = repository.AsProxy<Region>(regionId);
        }

        /// <summary>
        /// Gets the first compatible layout.
        /// </summary>
        /// <returns>Layout for blog post.</returns>
        private Layout GetFirstCompatibleLayout()
        {
            return
                repository.AsQueryable<Layout>()
                          .Where(layout =>
                              layout.LayoutRegions.Count(region => !region.IsDeleted && !region.Region.IsDeleted).Equals(1)
                                || layout.LayoutRegions.Any(region => !region.IsDeleted && !region.Region.IsDeleted && region.Region.RegionIdentifier == RegionIdentifier))
                          .FetchMany(layout => layout.LayoutRegions)
                          .ThenFetch(l => l.Region)
                          .ToList()
                          .FirstOrDefault();
        }

        /// <summary>
        /// Adds the default access rules to blog post entity.
        /// </summary>
        /// <param name="blogPost">The blog post.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="masterPage">The master page.</param>
        public void AddDefaultAccessRules(BlogPost blogPost, IPrincipal principal, Page masterPage)
        {
            // Set default access rules
            blogPost.AccessRules = new List<AccessRule>();

            if (masterPage != null)
            {
                blogPost.AccessRules = masterPage
                    .AccessRules
                    .Select(x => new AccessRule
                        {
                            Identity = x.Identity,
                            AccessLevel = x.AccessLevel,
                            IsForRole = x.IsForRole
                        })
                    .ToList();
            }
            else
            {
                var list = accessControlService.GetDefaultAccessList(principal);
                foreach (var rule in list)
                {
                    blogPost.AccessRules.Add(new AccessRule
                        {
                            Identity = rule.Identity,
                            AccessLevel = rule.AccessLevel,
                            IsForRole = rule.IsForRole
                        });
                }
            }
        }

        /// <summary>
        /// Gets the filtered blog posts query.
        /// </summary>
        /// <param name="request">The filter.</param>
        /// <param name="joinContents">if set to <c>true</c> join contents tables.</param>
        /// <returns>
        /// NHibernate query for getting filtered blog posts
        /// </returns>
        public NHibernate.IQueryOver<BlogPost, BlogPost> GetFilteredBlogPostsQuery(ViewModels.Filter.BlogsFilter request, bool joinContents = false)
        {
            request.SetDefaultSortingOptions("Title");

            BlogPost alias = null;

            var query = unitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Status != PageStatus.Preview);

            if (!request.IncludeArchived)
            {
                query = query.Where(() => !alias.IsArchived);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(Restrictions.Eq(Projections.Property(() => alias.Category.Id), request.CategoryId.Value));
            }

            if (request.LanguageId.HasValue)
            {
                if (request.LanguageId.Value.HasDefaultValue())
                {
                    query = query.Where(Restrictions.IsNull(Projections.Property(() => alias.Language.Id)));
                }
                else
                {
                    query = query.Where(Restrictions.Eq(Projections.Property(() => alias.Language.Id), request.LanguageId.Value));
                }
            }

            if (request.Tags != null)
            {
                foreach (var tagKeyValue in request.Tags)
                {
                    var id = tagKeyValue.Key.ToGuidOrDefault();
                    query = query.WithSubquery.WhereExists(QueryOver.Of<PageTag>().Where(tag => tag.Tag.Id == id && tag.Page.Id == alias.Id).Select(tag => 1));
                }
            }

            if (request.Status.HasValue)
            {
                if (request.Status.Value == PageStatusFilterType.OnlyPublished)
                {
                    query = query.Where(() => alias.Status == PageStatus.Published);
                }
                else if (request.Status.Value == PageStatusFilterType.OnlyUnpublished)
                {
                    query = query.Where(() => alias.Status != PageStatus.Published);
                }
                else if (request.Status.Value == PageStatusFilterType.ContainingUnpublishedContents)
                {
                    const ContentStatus draft = ContentStatus.Draft;
                    Root.Models.Content contentAlias = null;
                    var subQuery = QueryOver.Of<PageContent>()
                        .JoinAlias(p => p.Content, () => contentAlias)
                        .Where(pageContent => pageContent.Page.Id == alias.Id)
                        .And(() => contentAlias.Status == draft)
                        .And(() => !contentAlias.IsDeleted)
                        .Select(pageContent => 1);

                    query = query.WithSubquery.WhereExists(subQuery);
                }
            }

            if (request.SeoStatus.HasValue)
            {
                var subQuery = QueryOver.Of<SitemapNode>()
                    .Where(x => x.Page.Id == alias.Id || x.UrlHash == alias.PageUrlHash)
                    .And(x => !x.IsDeleted)
                    .JoinQueryOver(s => s.Sitemap)
                    .And(x => !x.IsDeleted)
                    .Select(s => 1);

                var hasSeoDisjunction =
                Restrictions.Disjunction()
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaTitle)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaKeywords)))
                    .Add(RestrictionsExtensions.IsNullOrWhiteSpace(Projections.Property(() => alias.MetaDescription)))
                    .Add(Subqueries.WhereNotExists(subQuery));

                if (request.SeoStatus.Value == SeoStatusFilterType.HasSeo)
                {
                    query = query.Where(Restrictions.Not(hasSeoDisjunction));
                }
                else
                {
                    query = query.Where(hasSeoDisjunction);
                }
            }

            if (joinContents)
            {
                PageContent pcAlias = null;
                BlogPostContent bcAlias = null;

                query = query.JoinAlias(() => alias.PageContents, () => pcAlias);
                query = query.JoinAlias(() => pcAlias.Content, () => bcAlias);
            }

            return query;
        }
    }
}