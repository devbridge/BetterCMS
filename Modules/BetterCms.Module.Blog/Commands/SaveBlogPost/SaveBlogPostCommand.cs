using System;
using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;

using NHibernate.Criterion;

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
        /// The redirect service
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveBlogPostCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="contentService">The content service.</param>
        public SaveBlogPostCommand(ITagService tagService, IOptionService optionService, IContentService contentService, IRedirectService redirectService)
        {
            this.tagService = tagService;
            this.optionService = optionService;
            this.contentService = contentService;
            this.redirectService = redirectService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Blog post view model</returns>
        public SaveBlogPostCommandResponse Execute(BlogPostViewModel request)
        {
            var layout = LoadLayout();
            var region = LoadRegion();
            var isNew = request.Id.HasDefaultValue();

            BlogPost blogPost;
            BlogPostContent content = null;
            PageContent pageContent = null;

            // Loading blog post and it's content, or creating new, if such not exists
            if (!isNew)
            {
                blogPost = Repository.First<BlogPost>(request.Id);
                content = Repository.FirstOrDefault<BlogPostContent>(c => c.PageContents.Any(x => x.Page == blogPost && x.Region == region && !x.IsDeleted));
                if (content != null)
                {
                    pageContent = Repository.FirstOrDefault<PageContent>(c => c.Page == blogPost && c.Region == region && !c.IsDeleted && c.Content == content);
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

            blogPost.Title = request.Title;
            blogPost.Description = request.IntroText;
            blogPost.Version = request.Version;
            if (isNew)
            {
                var parentPageUrl = request.ParentPageUrl.Trim('/');
                if (!string.IsNullOrWhiteSpace(parentPageUrl))
                {
                    var url = "/" + request.Title.Transliterate();
                    var pageUrl = string.Concat(parentPageUrl, url);
                    pageUrl = AddUrlPathSuffixIfNeeded(pageUrl);
                    blogPost.PageUrl = pageUrl;
                }
                else
                {
                    blogPost.PageUrl = GeneratePageUrl(request.Title);
                }
                blogPost.IsPublic = true;
                blogPost.Layout = layout;
            }

            // Push to change modified data each time save button is pressed
            blogPost.ModifiedOn = DateTime.Now;

            blogPost.Author = request.AuthorId.HasValue ? Repository.AsProxy<Author>(request.AuthorId.Value) : null;
            blogPost.Category = request.CategoryId.HasValue ? Repository.AsProxy<Category>(request.CategoryId.Value) : null;
            blogPost.Image = (request.Image != null && request.Image.ImageId.HasValue) ? Repository.AsProxy<MediaImage>(request.Image.ImageId.Value) : null;

            // Set content and save with status change
            content = new BlogPostContent
                          {
                              Id = content != null ? content.Id : Guid.Empty,
                              Name = request.Title,
                              Html = request.Content ?? string.Empty,
                              ActivationDate = request.LiveFromDate,
                              ExpirationDate = TimeHelper.FormatEndDate(request.LiveToDate)
                          };

            content = (BlogPostContent)contentService.SaveContentWithStatusUpdate(content, request.DesirableStatus);
            pageContent.Content = content;

            UpdateStatus(blogPost, request.DesirableStatus);

            Repository.Save(blogPost);
            Repository.Save(content);
            Repository.Save(pageContent);

            // Save tags
            tagService.SavePageTags(blogPost, request.Tags);

            // Commit
            UnitOfWork.Commit();

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

            Layout layout = layoutId.HasValue ? Repository.AsProxy<Layout>(layoutId.Value) : GetFirstCompatibleLayout();
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

        /// <summary>
        /// Generates the page URL.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>Url path</returns>
        private string GeneratePageUrl(string title)
        {
            var url = title.Transliterate();
            url = AddUrlPathSuffixIfNeeded(url);

            return url;
        }

        /// <summary>
        /// Path exists in db.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Url path.</returns>
        private bool PathExistsInDb(string url)
        {
            var exists = UnitOfWork.Session
                .QueryOver<Page>()
                .Where(p => !p.IsDeleted && p.PageUrl == url)
                .Select(p => p.Id)
                .RowCount();
            return exists > 0;
        }

        /// <summary>
        /// Adds the URL path suffix if needed.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Url path.</returns>
        private string AddUrlPathSuffixIfNeeded(string url)
        {
            var fullUrl = string.Format("/{0}/", url);

            // Check, if such record exists
            var exists = PathExistsInDb(fullUrl);

            if (exists)
            {
                // Load all titles
                var urlToReplace = string.Format("/{0}-", url);
                var urlToSearch = string.Format("{0}%", urlToReplace);
                Page alias = null;

                var paths = UnitOfWork.Session
                    .QueryOver(() => alias)
                    .Where(p => !p.IsDeleted)
                    .Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.PageUrl), urlToSearch))
                    .Select(p => p.PageUrl)
                    .List<string>();

                int maxNr = 0;
                var recheckInDb = false;
                foreach (var path in paths)
                {
                    int pathNr;
                    if (int.TryParse(path.Replace(urlToReplace, null).Trim('/'), out pathNr))
                    {
                        if (pathNr > maxNr)
                        {
                            maxNr = pathNr;
                        }
                    }
                    else
                    {
                        recheckInDb = true;
                    }
                }

                if (maxNr == int.MaxValue)
                {
                    recheckInDb = true;
                }
                else
                {
                    maxNr++;
                }

                if (string.IsNullOrWhiteSpace(url))
                {
                    fullUrl = "-";
                    recheckInDb = true;
                }
                else
                {
                    fullUrl = string.Format("/{0}-{1}/", url, maxNr);
                }

                if (recheckInDb)
                {
                    url = string.Format(fullUrl.Trim('/'));
                    return AddUrlPathSuffixIfNeeded(url);
                }
            }

            return fullUrl;
        }
    }
}