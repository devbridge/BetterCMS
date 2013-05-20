using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Exceptions.Service;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    /// <summary>
    /// Command saves existing or creates new blog post
    /// </summary>
    public class SaveBlogPostCommand : CommandBase, ICommand<BlogPostViewModel, SaveBlogPostCommandResponse>
    {
        /// <summary>
        /// The blog post region identifier.
        /// </summary>
        private const string regionIdentifier = BlogModuleConstants.BlogPostMainContentRegionIdentifier;

        /// <summary>
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// The option service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The content service
        /// </summary>
        private readonly IContentService contentService;

        /// <summary>
        /// The page service
        /// </summary>
        private readonly IPageService pageService;

        /// <summary>
        /// The blog service.
        /// </summary>
        private readonly IBlogService blogService;

        /// <summary>
        /// The url service.
        /// </summary>
        private readonly IUrlService urlService;
        
        /// <summary>
        /// The redirect service.
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveBlogPostCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="contentService">The content service.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="blogService">The blog service.</param>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="urlService">The URL service.</param>
        public SaveBlogPostCommand(ITagService tagService, IOptionService optionService, IContentService contentService, 
                                    IPageService pageService, IBlogService blogService, 
                                    IRedirectService redirectService, IUrlService urlService)
        {
            this.tagService = tagService;
            this.optionService = optionService;
            this.contentService = contentService;
            this.pageService = pageService;
            this.blogService = blogService;
            this.redirectService = redirectService;
            this.urlService = urlService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Blog post view model</returns>
        public SaveBlogPostCommandResponse Execute(BlogPostViewModel request)
        {
            if (request.DesirableStatus == ContentStatus.Published)
            {
                DemandAccess(RootModuleConstants.UserRoles.PublishContent);
            }

            var layout = LoadLayout();
            var region = LoadRegion();
            var isNew = request.Id.HasDefaultValue();
            var userCanEdit = false;

            if (isNew || request.DesirableStatus != ContentStatus.Published)
            {
                DemandAccess(RootModuleConstants.UserRoles.EditContent);
                userCanEdit = true;
            }
            else
            {
                userCanEdit = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent);
            }

            // UnitOfWork.BeginTransaction(); // NOTE: this causes concurrent data exception.

            BlogPost blogPost;
            BlogPostContent content = null;
            PageContent pageContent = null;
            Pages.Models.Redirect redirectCreated = null;

            // Loading blog post and it's content, or creating new, if such not exists
            if (!isNew)
            {
                blogPost = Repository.First<BlogPost>(request.Id);
                content = Repository.FirstOrDefault<BlogPostContent>(c => c.PageContents.Any(x => x.Page == blogPost && x.Region == region && !x.IsDeleted));
                if (content != null)
                {
                    pageContent = Repository.FirstOrDefault<PageContent>(c => c.Page == blogPost && c.Region == region && !c.IsDeleted && c.Content == content);
                }

                if (userCanEdit && !string.Equals(blogPost.PageUrl, request.BlogUrl, StringComparison.OrdinalIgnoreCase) && request.BlogUrl != null)
                {
                    request.BlogUrl = urlService.FixUrl(request.BlogUrl);
                    pageService.ValidatePageUrl(request.BlogUrl, request.Id);
                    if (request.RedirectFromOldUrl)
                    {
                        var redirect = redirectService.CreateRedirectEntity(blogPost.PageUrl, request.BlogUrl);
                        if (redirect != null)
                        {
                            Repository.Save(redirect);
                            redirectCreated = redirect;
                        }
                    }

                    blogPost.PageUrl = request.BlogUrl;
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
                blogPost.Author = request.AuthorId.HasValue ? Repository.AsProxy<Author>(request.AuthorId.Value) : null;
                blogPost.Category = request.CategoryId.HasValue ? Repository.AsProxy<Category>(request.CategoryId.Value) : null;
                blogPost.Image = (request.Image != null && request.Image.ImageId.HasValue) ? Repository.AsProxy<MediaImage>(request.Image.ImageId.Value) : null;
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
                    blogPost.PageUrl = blogService.CreateBlogPermalink(request.Title);
                }               
               
                blogPost.IsPublic = true;
                blogPost.Layout = layout;
                UpdateStatus(blogPost, request.DesirableStatus);
            }
            else if (request.DesirableStatus == ContentStatus.Published)
            {
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

            Repository.Save(blogPost);
            Repository.Save(content);
            Repository.Save(pageContent);

            // Save tags if user has edit right.
            IList<Tag> newTags = null;
            if (userCanEdit)
            {
                tagService.SavePageTags(blogPost, request.Tags, out newTags);
            }

            // Commit
            UnitOfWork.Commit();

            // Notify about new or updated blog post.
            if (isNew)
            {
                BlogsApiContext.Events.OnBlogCreated(blogPost);
            }
            else
            {
                BlogsApiContext.Events.OnBlogUpdated(blogPost);
            }

            // Notify about new created tags.
            PagesApiContext.Events.OnTagCreated(newTags);

            // Notify about redirect creation.
            if (redirectCreated != null)
            {
                PagesApiContext.Events.OnRedirectCreated(redirectCreated);
            }

            return new SaveBlogPostCommandResponse
                       {
                           Id = blogPost.Id,
                           Version = blogPost.Version,
                           Title = blogPost.Title,
                           PageUrl = blogPost.PageUrl,
                           ModifiedByUser = blogPost.ModifiedByUser,
                           ModifiedOn = blogPost.ModifiedOn.ToFormattedDateString(),
                           CreatedOn = blogPost.CreatedOn.ToFormattedDateString(),
                           PageStatus = blogPost.Status,
                           DesirableStatus = request.DesirableStatus,
                           PageContentId = pageContent.Id
                       };
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
        /// <returns>Layout for blog post.</returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">If layout was not found.</exception>
        private Layout LoadLayout()
        {
            var layoutId = optionService.GetDefaultTemplateId();

            var layout = (layoutId.HasValue
                                 ? Repository.AsQueryable<Layout>(l => l.Id == layoutId.Value).FirstOrDefault()
                                 : null) ?? GetFirstCompatibleLayout();

            if (layout == null)
            {
                var message = BlogGlobalization.SaveBlogPost_LayoutNotFound_Message;
                const string logMessage = "Failed to save blog post. No compatible layouts found.";
                throw new ValidationException(() => message, logMessage);
            }

            return layout;
        }

        /// <summary>
        /// Loads the region.
        /// </summary>
        /// <returns>Region for blog post content.</returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">If no region found.</exception>
        private Region LoadRegion()
        {
            var regionId = Repository.AsQueryable<Region>(r => !r.IsDeleted && r.RegionIdentifier == regionIdentifier).Select(s => s.Id).FirstOrDefault();
            if (regionId.HasDefaultValue())
            {
                var message = string.Format(BlogGlobalization.SaveBlogPost_RegionNotFound_Message, regionIdentifier);
                var logMessage = string.Format("Region {0} for rendering blog post content not found.", regionIdentifier);
                throw new ValidationException(() => message, logMessage);
            }

            var region = Repository.AsProxy<Region>(regionId);

            return region;
        }

        /// <summary>
        /// Gets the first compatible layout.
        /// </summary>
        /// <returns>Layout for blog post.</returns>
        private Layout GetFirstCompatibleLayout()
        {
            LayoutRegion layoutRegionAlias = null;
            Region regionAlias = null;

            var compatibleLayouts = UnitOfWork.Session
               .QueryOver(() => layoutRegionAlias)
               .Inner.JoinQueryOver(() => layoutRegionAlias.Region, () => regionAlias)
               .Where(() => !layoutRegionAlias.IsDeleted
                   && !regionAlias.IsDeleted
                   && regionAlias.RegionIdentifier == BlogModuleConstants.BlogPostMainContentRegionIdentifier)
               .Select(select => select.Layout.Id)
               .Take(1)
               .List<Guid>();
            
            if (compatibleLayouts != null && compatibleLayouts.Count > 0)
            {
                return Repository.AsProxy<Layout>(compatibleLayouts[0]);
            }

            return null;
        }
    }
}