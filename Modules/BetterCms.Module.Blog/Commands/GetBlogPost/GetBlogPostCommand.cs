using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Transform;

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
        /// Initializes a new instance of the <see cref="GetBlogPostCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="authorService">The author service.</param>
        /// <param name="tagService">The tag service.</param>
        public GetBlogPostCommand(ICategoryService categoryService, IAuthorService authorService, ITagService tagService)
        {
            this.categoryService = categoryService;
            this.authorService = authorService;
            this.tagService = tagService;
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
                BlogPostViewModel blogModelAlias = null;

                model = Repository.AsQueryable<BlogPost>()
                    .Where(bp => bp.Id == id)
                    .Select(bp =>
                        new BlogPostViewModel
                            {
                                Id = bp.Id,
                                Version = bp.Version,
                                Title = bp.Title,
                                IntroText = bp.Description,
                                AuthorId = bp.Author.Id,
                                CategoryId = bp.Category.Id,
                                Image = bp.Image != null ?
                                    new ImageSelectorViewModel
                                    {
                                        ImageId = bp.Image.Id,
                                        ImageVersion = bp.Image.Version,
                                        ImageUrl = bp.Image.PublicUrl,
                                        ThumbnailUrl = bp.Image.PublicThumbnailUrl,
                                        ImageTooltip = bp.Image.Caption
                                    } : new ImageSelectorViewModel()
                            })
                    .FirstOrDefault();

                if (model != null)
                {
                    var regionId = UnitOfWork.Session
                        .QueryOver<Region>()
                        .Where(r => r.RegionIdentifier == BlogModuleConstants.BlogPostMainContentRegionIdentifier)
                        .Select(r => r.Id)
                        .FutureValue<Guid>();

                    PageContent pageContentAlias = null;
                    BlogPostContent blogPostContentAlias = null;

                    var contents = UnitOfWork.Session
                        .QueryOver(() => blogPostContentAlias)
                        .Inner.JoinAlias(c => c.PageContents, () => pageContentAlias)
                        .Where(() => pageContentAlias.Page.Id == id
                            && !pageContentAlias.IsDeleted
                            && pageContentAlias.Region.Id == regionId.Value)
                        .OrderBy(() => pageContentAlias.CreatedOn).Asc
                        .SelectList(select => select
                            .Select(() => blogPostContentAlias.Html).WithAlias(() => blogModelAlias.Content)
                            .Select(() => blogPostContentAlias.ActivationDate).WithAlias(() => blogModelAlias.LiveFromDate)
                            .Select(() => blogPostContentAlias.ExpirationDate).WithAlias(() => blogModelAlias.LiveToDate))
                        .TransformUsing(Transformers.AliasToBean<BlogPostViewModel>())
                        .Take(1)
                        .List<BlogPostViewModel>();

                    if (contents != null && contents.Count > 0)
                    {
                        var content = contents[0];
                        model.Content = content.Content;
                        model.LiveFromDate = content.LiveFromDate;
                        model.LiveToDate = content.LiveToDate;
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

            return model;
        }
    }
}