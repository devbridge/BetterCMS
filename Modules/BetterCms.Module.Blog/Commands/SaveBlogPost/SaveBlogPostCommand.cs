using System;
using System.Linq;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;

using BetterCms.Module.MediaManager.Models;

using BetterCms.Module.Pages.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using NHibernate.Criterion;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    /// <summary>
    /// Command saves existing or creates new blog post
    /// </summary>
    public class SaveBlogPostCommand : CommandBase, ICommand<BlogPostViewModel, SaveBlogPostCommandResponse>
    {
        /// <summary>
        /// The blog post region identifier
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
        /// Initializes a new instance of the <see cref="SaveBlogPostCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="optionService">The option service.</param>
        public SaveBlogPostCommand(ITagService tagService, IOptionService optionService)
        {
            this.tagService = tagService;
            this.optionService = optionService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Blog post view model</returns>
        public SaveBlogPostCommandResponse Execute(BlogPostViewModel request)
        {
            // Load layout
            Layout layout;
            var layoutId = optionService.GetDefaultTemplateId();

            if (layoutId.HasValue)
            {
                layout = Repository.AsProxy<Layout>(layoutId.Value);
            } 
            else
            {
                layout = GetFirstCompatibleLayout();
            }
            if (layout == null)
            {
                var message = BlogGlobalization.SaveBlogPost_LayoutNotFound_Message;
                var logMessage = "Failed to save blog post. No compatible layouts found.";
                throw new ValidationException(e => message, logMessage);
            }

            // Loading region
            var regionId = Repository.AsQueryable<Region>(r => !r.IsDeleted && r.RegionIdentifier == regionIdentifier).Select(s => s.Id).FirstOrDefault();
            if (regionId.HasDefaultValue())
            {
                var message = string.Format(BlogGlobalization.SaveBlogPost_RegionNotFound_Message, regionIdentifier);
                var logMessage = string.Format("Region {0} for rendering blog post content not found.", regionIdentifier);
                throw new ValidationException(e => message, logMessage);
            }
            var region = Repository.AsProxy<Region>(regionId);

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

            if (content == null)
            {
                content = new BlogPostContent { Name = request.Title };
            }

            if (pageContent == null)
            {
                pageContent = new PageContent { Region = region, Content = content, Page = blogPost };

                // TODO: set correct status
                pageContent.Status = ContentStatus.Published;
            }

            blogPost.Title = request.Title;
            blogPost.Description = request.IntroText;
            blogPost.Version = request.Version;

            if (isNew)
            {
                blogPost.PageUrl = GeneratePageUrl(request.Title);
                blogPost.IsPublic = true;
                blogPost.Layout = layout;

                // TODO: set correct status
                blogPost.IsPublished = true;
                blogPost.PublishedOn = DateTime.Now;
            }

            if (request.AuthorId.HasValue)
            {
                blogPost.Author = Repository.AsProxy<Author>(request.AuthorId.Value);
            }
            else
            {
                blogPost.Author = null;
            }

            if (request.CategoryId.HasValue)
            {
                blogPost.Category = Repository.AsProxy<Category>(request.CategoryId.Value);
            }
            else
            {
                blogPost.Category = null;
            }

            if (request.Image != null && request.Image.ImageId.HasValue)
            {
                blogPost.Image = Repository.AsProxy<MediaImage>(request.Image.ImageId.Value);
            }
            else
            {
                blogPost.Image = null;
            }

            // Set content
            if (string.IsNullOrWhiteSpace(content.Name))
            {
                content.Name = request.Title;
            }
            content.Html = request.Content ?? string.Empty;
            content.ActivationDate = request.LiveFromDate;
            content.ExpirationDate = request.LiveToDate;

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
                           IsPublished = blogPost.IsPublished
                       };
        }

        /// <summary>
        /// Gets the first compatible layout.
        /// </summary>
        /// <returns>Layout</returns>
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
        /// Pathes the exists in db.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Url path</returns>
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
        /// Adds the URL path sufix if needed.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Url path</returns>
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