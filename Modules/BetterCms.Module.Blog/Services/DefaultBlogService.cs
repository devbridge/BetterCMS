using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Models.Events;
using BetterCms.Module.Blog.ViewModels.Blog;

using BetterCms.Module.MediaManager.Models;

using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Filter;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using Common.Logging;

using NHibernate.Criterion;
using NHibernate.Linq;

using RootOptionService = BetterCms.Module.Root.Services.IOptionService;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultBlogService : IBlogService
    {
        /// <summary>
        /// The blog post region identifier.
        /// </summary>
        private const string RegionIdentifier = BlogModuleConstants.BlogPostMainContentRegionIdentifier;

        /// <summary>
        /// The category service
        /// </summary>
        private ICategoryService categoryService;

        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        protected readonly ICmsConfiguration configuration;
        private readonly IUrlService urlService;
        protected readonly IRepository repository;
        private readonly IOptionService blogOptionService;
        private readonly RootOptionService optionService;
        protected readonly IAccessControlService accessControlService;
        private readonly ISecurityService securityService;
        protected readonly IContentService contentService;
        private readonly IPageService pageService;
        private readonly IRedirectService redirectService;
        protected readonly IMasterPageService masterPageService;
        private readonly ITagService tagService;
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultBlogService" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="blogOptionService">The blog option service.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="contentService">The content service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="masterPageService">The master page service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="categoryService">The category service.</param>
        public DefaultBlogService(ICmsConfiguration configuration, IUrlService urlService, IRepository repository,
            IOptionService blogOptionService, IAccessControlService accessControlService, ISecurityService securityService,
            IContentService contentService, ITagService tagService,
            IPageService pageService, IRedirectService redirectService, IMasterPageService masterPageService,
            IUnitOfWork unitOfWork, RootOptionService optionService, ICategoryService categoryService)
        {
            this.configuration = configuration;
            this.urlService = urlService;
            this.repository = repository;
            this.blogOptionService = blogOptionService;
            this.optionService = optionService;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
            this.contentService = contentService;
            this.pageService = pageService;
            this.redirectService = redirectService;
            this.masterPageService = masterPageService;
            this.tagService = tagService;
            this.unitOfWork = unitOfWork;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Creates the blog URL from the given blog title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="unsavedUrls">The unsaved urls.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>
        /// Created blog URL
        /// </returns>
        public string CreateBlogPermalink(string title, List<string> unsavedUrls = null, IEnumerable<Guid> categoryId = null)
        {
            string newUrl = null;
            if (UrlHelper.GeneratePageUrl != null)
            {
                try
                {
                    newUrl =
                        UrlHelper.GeneratePageUrl(
                            new PageUrlGenerationRequest
                            {
                                Title = title,
                                CategoryId = categoryId
                            });
                }
                catch (Exception ex)
                {
                    Logger.Error("Custom blog post url generation failed.", ex);
                }
            }

            if (string.IsNullOrWhiteSpace(newUrl))
            {
                newUrl = CreateBlogPermalinkDefault(title, unsavedUrls);
            }
            else
            {
                newUrl = urlService.AddPageUrlPostfix(newUrl, "{0}");
            }

            return newUrl;
        }

        /// <summary>
        /// Creates the blog permalink default.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="unsavedUrls">The unsaved urls.</param>
        /// <returns>
        /// Created blog URL
        /// </returns>
        private string CreateBlogPermalinkDefault(string title, List<string> unsavedUrls = null)
        {
            var url = title.Transliterate(true);
            url = urlService.AddPageUrlPostfix(url, configuration.ArticleUrlPattern, unsavedUrls);

            return url;
        }

        /// <summary>
        /// Saves the blog post.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="childContentOptionValues">The child content option values.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <param name="updateActivationIfNotChanged">if set to <c>true</c> update activation time even if it was not changed.</param>
        /// <returns>
        /// Saved blog post entity
        /// </returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        /// <exception cref="SecurityException">Forbidden: Access is denied.</exception>
        public BlogPost SaveBlogPost(BlogPostViewModel request, IList<ContentOptionValuesViewModel> childContentOptionValues, IPrincipal principal, out string[] errorMessages, bool updateActivationIfNotChanged = true)
        {
            errorMessages = new string[0];
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

            var isNew = request.Id.HasDefaultValue();
            var userCanEdit = securityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent);

            ValidateData(isNew, request);

            BlogPost blogPost;
            BlogPostContent content;
            PageContent pageContent;
            GetBlogPostAndContentEntities(request, principal, roles, ref isNew, out content, out pageContent, out blogPost);
            var beforeChange = new UpdatingBlogModel(blogPost);

            // Master page / layout
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

            if (pageContent.Region == null)
            {
                pageContent.Region = region;
            }

            // Load master pages for updating page's master path and page's children master path
            IList<Guid> newMasterIds;
            IList<Guid> oldMasterIds;
            IList<Guid> childrenPageIds;
            IList<MasterPage> existingChildrenMasterPages;
            PrepareForUpdateChildrenMasterPages(isNew, blogPost, request, out newMasterIds, out oldMasterIds, out childrenPageIds, out existingChildrenMasterPages);

            // TODO: TEST AND TRY TO FIX IT: TRANSACTION HERE IS REQUIRED!
            // UnitOfWork.BeginTransaction(); // NOTE: this causes concurrent data exception.

            Redirect redirectCreated = null;
            if (!isNew && userCanEdit && !string.Equals(blogPost.PageUrl, request.BlogUrl) && !string.IsNullOrWhiteSpace(request.BlogUrl))
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

            // Push to change modified data each time.
            blogPost.ModifiedOn = DateTime.Now;

            if (userCanEdit)
            {
                blogPost.Title = request.Title;
                blogPost.Description = request.IntroText;
                blogPost.Author = request.AuthorId.HasValue ? repository.AsProxy<Author>(request.AuthorId.Value) : null;
                
                blogPost.Image = (request.Image != null && request.Image.ImageId.HasValue) ? repository.AsProxy<MediaImage>(request.Image.ImageId.Value) : null;
                if (isNew || request.DesirableStatus == ContentStatus.Published)
                {
                    if (updateActivationIfNotChanged)
                    {
                        blogPost.ActivationDate = request.LiveFromDate;
                        blogPost.ExpirationDate = TimeHelper.FormatEndDate(request.LiveToDate);
                    }
                    else
                    {
                        blogPost.ActivationDate = TimeHelper.GetFirstIfTheSameDay(blogPost.ActivationDate, request.LiveFromDate);
                        blogPost.ExpirationDate = TimeHelper.GetFirstIfTheSameDay(blogPost.ExpirationDate, TimeHelper.FormatEndDate(request.LiveToDate));
                    }
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
                    blogPost.PageUrl = CreateBlogPermalink(request.Title, null, request.Categories != null ? request.Categories.Select(c => Guid.Parse(c.Key)) : null);
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
                OriginalText = request.OriginalText,
                EditInSourceMode = request.EditInSourceMode,
                ActivationDate = request.LiveFromDate,
                ExpirationDate = TimeHelper.FormatEndDate(request.LiveToDate),
                ContentTextMode = request.ContentTextMode,
            };

            if (!updateActivationIfNotChanged && content != null)
            {
                newContent.ActivationDate = TimeHelper.GetFirstIfTheSameDay(content.ActivationDate, newContent.ActivationDate);
                newContent.ExpirationDate = TimeHelper.GetFirstIfTheSameDay(content.ExpirationDate, newContent.ExpirationDate);
            }

            if (request.ContentTextMode == ContentTextMode.Markdown
                && request.Content == null
                && request.OriginalText != null)
            {
                newContent.Html = MarkdownConverter.ToHtml(request.OriginalText);
            }

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
                newContent.ContentTextMode = contentToPublish.ContentTextMode;
                newContent.OriginalText = contentToPublish.OriginalText;
            }

            content = SaveContentWithStatusUpdate(isNew, newContent, request, principal);
            pageContent.Content = content;
            optionService.SaveChildContentOptions(content, childContentOptionValues, request.DesirableStatus);
            
            blogPost.PageUrlHash = blogPost.PageUrl.UrlHash();
            blogPost.UseCanonicalUrl = request.UseCanonicalUrl;

            MapExtraProperties(isNew, blogPost, content, pageContent, request, principal);

            // Notify about page properties changing.
            var cancelEventArgs = Events.BlogEvents.Instance.OnBlogChanging(beforeChange, new UpdatingBlogModel(blogPost));
            if (cancelEventArgs.Cancel)
            {
                errorMessages = cancelEventArgs.CancellationErrorMessages.ToArray();
                return null;
            }

            if (isNew || userCanEdit)
            {
                categoryService.CombineEntityCategories<BlogPost, PageCategory>(blogPost, request.Categories);
            }

            repository.Save(blogPost);
            repository.Save(content);
            repository.Save(pageContent);

            masterPageService.UpdateChildrenMasterPages(existingChildrenMasterPages, oldMasterIds, newMasterIds, childrenPageIds);

            pageContent.Content = content;
            blogPost.PageContents = new [] {pageContent};

            IList<Tag> newTags = null;
            if (userCanEdit)
            {
                newTags = SaveTags(blogPost, request);
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

        protected virtual IList<Tag> SaveTags(BlogPost blogPost, BlogPostViewModel request)
        {
            IList<Tag> newTags;
            tagService.SavePageTags(blogPost, request.Tags, out newTags);

            return newTags;
        }

        protected virtual BlogPostContent SaveContentWithStatusUpdate(bool isNew, BlogPostContent newContent, BlogPostViewModel request, IPrincipal principal)
        {
            return (BlogPostContent)contentService.SaveContentWithStatusUpdate(newContent, request.DesirableStatus);
        }

        protected virtual void PrepareForUpdateChildrenMasterPages(bool isNew, BlogPost entity, BlogPostViewModel model, out IList<Guid> newMasterIds,
            out IList<Guid> oldMasterIds, out IList<Guid> childrenPageIds, out IList<MasterPage> existingChildrenMasterPages)
        {
            newMasterIds = null;
            oldMasterIds = null;
            childrenPageIds = null;
            existingChildrenMasterPages = null;
        }

        protected virtual void GetBlogPostAndContentEntities(BlogPostViewModel request, IPrincipal principal, string[] roles, 
            ref bool isNew, out BlogPostContent content, out PageContent pageContent , out BlogPost blogPost)
        {
            content = null;
            pageContent = null;

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

                if (configuration.Security.AccessControlEnabled)
                {
                    accessControlService.DemandAccess(blogPost, principal, AccessLevel.ReadWrite, roles);
                }

                if (content != null)
                {
                    // Check if user has confirmed the deletion of content
                    if (!request.IsUserConfirmed && blogPost.IsMasterPage)
                    {
                        contentService.CheckIfContentHasDeletingChildrenWithException(blogPost.Id, content.Id, request.Content);
                    }

                    var bpRef = blogPost;
                    var contentRef = content;
                    pageContent = repository.FirstOrDefault<PageContent>(c => c.Page == bpRef && !c.IsDeleted && c.Content == contentRef);
                }
            }
            else
            {
                blogPost = new BlogPost();
            }

            if (pageContent == null)
            {
                pageContent = new PageContent { Page = blogPost };
            }
        }

        protected virtual void MapExtraProperties(bool isNew, BlogPost entity, BlogPostContent content, PageContent pageContent, BlogPostViewModel model, IPrincipal principal)
        {
            entity.Version = model.Version;
        }

        protected virtual void ValidateData(bool isNew, BlogPostViewModel model)
        {
            // Do nothing
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
                    if (blogPost.Status != PageStatus.Published)
                    {
                        blogPost.PublishedOn = DateTime.Now;
                    }
                    blogPost.Status = PageStatus.Published;
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

        private void LoadDefaultLayoutAndRegion(out Layout layout, out Page masterPage, out Region region)
        {
            var option = blogOptionService.GetDefaultOption();

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
                var regionIdentifier = RegionIdentifier.ToLowerInvariant();
                regionId = layout.LayoutRegions.Count(layoutRegion => !layoutRegion.IsDeleted && !layoutRegion.Region.IsDeleted) == 1
                                   ? layout.LayoutRegions.First(layoutRegion => !layoutRegion.IsDeleted).Region.Id
                                   : layout.LayoutRegions.Where(
                                       layoutRegion =>
                                       !layoutRegion.IsDeleted && !layoutRegion.Region.IsDeleted && layoutRegion.Region.RegionIdentifier.ToLowerInvariant() == regionIdentifier)
                                           .Select(layoutRegion => layoutRegion.Region.Id)
                                           .FirstOrDefault();
            }
            else
            {
                var masterPageRef = masterPage;
                var results = repository.AsQueryable<PageContent>()
                          .Where(pageContent => pageContent.Page == masterPageRef)
                          .SelectMany(pageContent => pageContent.Content.ContentRegions)
                          .Select(contentRegion => new { contentRegion.Region.Id, contentRegion.Region.RegionIdentifier })
                          .ToList();
                var mainContent = results.FirstOrDefault(r => r.RegionIdentifier.ToLowerInvariant() == RegionIdentifier.ToLowerInvariant());
                if (mainContent == null)
                {
                    mainContent = results.FirstOrDefault();
                }

                if (mainContent != null)
                {
                    regionId = mainContent.Id;
                }
                else
                {
                    regionId = Guid.Empty;
                }
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
            var regionIdentifier = RegionIdentifier.ToLowerInvariant();

            return
                repository.AsQueryable<Layout>()
                          .Where(layout =>
                              layout.LayoutRegions.Count(region => !region.IsDeleted && !region.Region.IsDeleted).Equals(1)
                                || layout.LayoutRegions.Any(region => !region.IsDeleted && !region.Region.IsDeleted && region.Region.RegionIdentifier.ToLowerInvariant() == regionIdentifier))
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
        protected void AddDefaultAccessRules(BlogPost blogPost, IPrincipal principal, Page masterPage)
        {
            IEnumerable<IAccessRule> accessRules;

            if (masterPage != null)
            {
                accessRules = masterPage.AccessRules;
            }
            else
            {
                accessRules = accessControlService.GetDefaultAccessList(principal);
            }

            accessControlService.UpdateAccessControl(blogPost, accessRules.ToList());
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

            if (request.Categories != null)
            {
                var categories = request.Categories.Select(c => new Guid(c.Key)).Distinct().ToList();

                foreach (var category in categories)
                {
                    var childCategories = categoryService.GetChildCategoriesIds(category).ToArray();
                    query = query.WithSubquery.WhereExists(QueryOver.Of<PageCategory>().Where(cat => !cat.IsDeleted && cat.Page.Id == alias.Id).WhereRestrictionOn(cat => cat.Category.Id).IsIn(childCategories).Select(cat => 1));
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