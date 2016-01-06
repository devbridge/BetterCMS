using System;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using BlogContent = BetterCms.Module.Root.Models.Content;
using ITagService = BetterCms.Module.Pages.Services.ITagService;

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
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The blog option service
        /// </summary>
        private readonly Services.IOptionService blogOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="authorService">The author service.</param>
        /// <param name="tagService">The tag service.</param>
        /// <param name="contentService">The content service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="blogOptionService">The blog option service.</param>
        public GetBlogPostCommand(ICategoryService categoryService, IAuthorService authorService,
            ITagService tagService, IContentService contentService, IMediaFileUrlResolver fileUrlResolver,
            ICmsConfiguration cmsConfiguration, Services.IOptionService blogOptionService)
        {
            this.categoryService = categoryService;
            this.authorService = authorService;
            this.tagService = tagService;
            this.contentService = contentService;
            this.fileUrlResolver = fileUrlResolver;
            this.cmsConfiguration = cmsConfiguration;
            this.blogOptionService = blogOptionService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="id">The page id.</param>
        /// <returns>Blog post model for create / edit</returns>
        public BlogPostViewModel Execute(Guid id)
        {
            var model = new BlogPostViewModel();
            var categoriesFilterKey = BlogPost.CategorizableItemKeyForBlogs;

            if (!id.HasDefaultValue())
            {               
                var result = Repository.AsQueryable<BlogPost>()
                    .Where(bp => bp.Id == id)
                    .Select(bp => new {
                            Entity = bp,
                            AccessRules = bp.AccessRules,
                            Model = new BlogPostViewModel
                                {
                                    Id = bp.Id,
                                    Version = bp.Version,
                                    Title = bp.Title,
                                    BlogUrl = bp.PageUrl,
                                    UseCanonicalUrl = bp.UseCanonicalUrl,
                                    IntroText = bp.Description,
                                    AuthorId = bp.Author != null ? bp.Author.Id : (Guid?)null,                                  
                                    Image = bp.Image == null || bp.Image.IsDeleted ? null :
                                        new ImageSelectorViewModel
                                        {
                                            ImageId = bp.Image.Id,
                                            ImageVersion = bp.Image.Version,
                                            ImageUrl = fileUrlResolver.EnsureFullPathUrl(bp.Image.PublicUrl),
                                            ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(bp.Image.PublicThumbnailUrl),
                                            ImageTooltip = bp.Image.Caption,
                                            FolderId = bp.Image.Folder != null ? bp.Image.Folder.Id : (Guid?)null
                                        }
                                }
                            })
                    .ToList()
                    .FirstOne();
                model = result.Model;

                if (model != null)
                {
                    categoriesFilterKey = result.Entity.GetCategorizableItemKey();
                    if (cmsConfiguration.Security.AccessControlEnabled)
                    {
                        SetIsReadOnly(model, result.AccessRules.Cast<IAccessRule>().ToList());
                    }

                    var pageContentId = Repository.AsQueryable<PageContent>()
                        .Where(pageContent => pageContent.Page.Id == id && !pageContent.Page.IsDeleted && pageContent.Content is BlogPostContent)
                        .Select(pageContent => pageContent.Id)
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
                        model.ContentId = content.Id;
                        model.ContentVersion = content.Version;
                        model.LiveFromDate = content.ActivationDate;
                        model.LiveToDate = content.ExpirationDate;
                        model.EditInSourceMode = content.EditInSourceMode;
                        model.CurrentStatus = content.Status;
                        model.HasPublishedContent = content.Original != null;
                        model.ContentTextMode = content.ContentTextMode;
                        model.Content = content.ContentTextMode == ContentTextMode.Html ? content.Html : content.OriginalText;
                    }
                    else
                    {
                        model.LiveFromDate = DateTime.Today;
                    }
                    model.Categories = categoryService.GetSelectedCategories<BlogPost, PageCategory>(id).ToList();
                    model.Tags = tagService.GetPageTagNames(id).ToList();
                }
                else
                {
                    model = new BlogPostViewModel();
                }
            }
            else
            {
                model.LiveFromDate = DateTime.Today;

                var option = blogOptionService.GetDefaultOption();
                if (option != null)
                {
                    model.ContentTextMode = option.DefaultContentTextMode;
                }
            }

            model.Authors = authorService.GetAuthors();
            model.RedirectFromOldUrl = true;
            model.CategoriesFilterKey = categoriesFilterKey;

            return model;
        }
    }
}