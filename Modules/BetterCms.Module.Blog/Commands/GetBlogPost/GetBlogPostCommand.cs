using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BlogContent = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Blog.Commands.GetBlogPost
{
    /// <summary>
    /// Command for getting blog post view model
    /// </summary>
    public class GetBlogPostCommand : CommandBase, ICommand<Guid, BlogPostViewModel>
    {
        /// <summary>
        /// The category service
        /// </summary>
        private ICategoryService categoryService;

        /// <summary>
        /// The author service
        /// </summary>
        private IAuthorService authorService;

        /// <summary>
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// The content service
        /// </summary>
        private readonly IContentService contentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="authorService">The author service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="contentService">The content service.</param>
        public GetBlogPostCommand(ICategoryService categoryService, IAuthorService authorService,
            ITagService tagService, IContentService contentService)
        {
            this.categoryService = categoryService;
            this.authorService = authorService;
            this.tagService = tagService;
            this.contentService = contentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="id">The page id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public BlogPostViewModel Execute(Guid id)
        {
            var model = new BlogPostViewModel();

            if (!id.HasDefaultValue())
            {
                model = Repository.AsQueryable<BlogPost>()
                    .Where(bp => bp.Id == id)
                    .Select(bp =>
                        new BlogPostViewModel
                            {
                                Id = bp.Id,
                                Version = bp.Version,
                                Title = bp.Title,
                                BlogUrl = bp.PageUrl,
                                UseCanonicalUrl = bp.UseCanonicalUrl,
                                IntroText = bp.Description,
                                AuthorId = bp.Author.Id,
                                CategoryId = bp.Category.Id,
                                Image = bp.Image == null ? null :
                                    new ImageSelectorViewModel
                                    {
                                        ImageId = bp.Image.Id,
                                        ImageVersion = bp.Image.Version,
                                        ImageUrl = bp.Image.PublicUrl,
                                        ThumbnailUrl = bp.Image.PublicThumbnailUrl,
                                        ImageTooltip = bp.Image.Caption,
                                        FolderId = bp.Image.Folder != null ? bp.Image.Folder.Id : (Guid?)null
                                    }
                            })
                    .FirstOne();

                if (model != null)
                {
                    var regionId = UnitOfWork.Session
                        .QueryOver<Region>()
                        .Where(r => r.RegionIdentifier == BlogModuleConstants.BlogPostMainContentRegionIdentifier)
                        .Select(r => r.Id)
                        .FutureValue<Guid>();

                    PageContent pageContentAlias = null;
                    BlogPostContent blogPostContentAlias = null;

                    var pageContentId = UnitOfWork.Session
                        .QueryOver(() => pageContentAlias)
                        .Inner.JoinAlias(c => c.Content, () => blogPostContentAlias)
                        .Where(() => pageContentAlias.Page.Id == id
                            && !pageContentAlias.IsDeleted
                            && pageContentAlias.Region.Id == regionId.Value)
                        .OrderBy(() => pageContentAlias.CreatedOn).Asc
                        .Select(c => c.Id)
                        .Take(1)
                        .List<Guid>()
                        .FirstOrDefault();

                    BlogPostContent content = null;
                    if (!pageContentId.HasDefaultValue())
                    {
                        var pageContent = contentService.GetPageContentForEdit(pageContentId);
                        if (pageContent != null)
                        {
                            content = pageContent.Item2 as BlogPostContent;
                        }
                    }

                    if (content != null)
                    {
                        model.Content = content.Html;
                        model.ContentId = content.Id;
                        model.ContentVersion = content.Version;
                        model.LiveFromDate = content.ActivationDate;
                        model.LiveToDate = content.ExpirationDate;
                        model.EditInSourceMode = content.EditInSourceMode;
                        model.CurrentStatus = content.Status;
                        model.HasPublishedContent = content.Original != null;
                    }
                    else
                    {
                        model.LiveFromDate = DateTime.Today;
                    }

                    model.Tags = tagService.GetPageTagNames(id);
                }
                else
                {
                    model = new BlogPostViewModel();
                }
            }
            else
            {
                model.LiveFromDate = DateTime.Today;
            }

            model.Authors = authorService.GetAuthors();
            model.Categories = categoryService.GetCategories();
            model.RedirectFromOldUrl = true;

            return model;
        }
    }
}