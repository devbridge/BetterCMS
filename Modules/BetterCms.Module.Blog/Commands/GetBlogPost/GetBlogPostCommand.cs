using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Models;
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
        /// Initializes a new instance of the <see cref="GetBlogPostCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="authorService">The author service.</param>
        public GetBlogPostCommand(ICategoryService categoryService, IAuthorService authorService)
        {
            this.categoryService = categoryService;
            this.authorService = authorService;
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
                BlogPost blogAlias = null;
                BlogPostViewModel blogModelAlias = null;

                model = UnitOfWork.Session
                    .QueryOver(() => blogAlias)
                    .Where(() => !blogAlias.IsDeleted && blogAlias.Id == id)
                    .SelectList(select => select
                        .Select(() => blogAlias.Id).WithAlias(() => blogModelAlias.Id)
                        .Select(() => blogAlias.Title).WithAlias(() => blogModelAlias.Title)
                        .Select(() => blogAlias.Version).WithAlias(() => blogModelAlias.Version)
                        .Select(() => blogAlias.Description).WithAlias(() => blogModelAlias.IntroText)
                        .Select(() => blogAlias.Author.Id).WithAlias(() => blogModelAlias.AuthorId)
                        .Select(() => blogAlias.Category.Id).WithAlias(() => blogModelAlias.CategoryId))
                    .TransformUsing(Transformers.AliasToBean<BlogPostViewModel>())
                    .SingleOrDefault<BlogPostViewModel>();

                if (model != null)
                {
                    var regionId = UnitOfWork.Session
                        .QueryOver<Region>()
                        .Where(r => r.RegionIdentifier == BlogModuleConstants.BlogPostMainContentRegionIdentifier)
                        .Select(r => r.Id)
                        .FutureValue<Guid>();

                    PageContent pageContentAlias = null;
                    HtmlContent htmlContentAlias = null;

                    var contents = UnitOfWork.Session
                        .QueryOver(() => htmlContentAlias)
                        .Inner.JoinAlias(c => c.PageContents, () => pageContentAlias)
                        .Where(() => pageContentAlias.Page.Id == id
                            && !pageContentAlias.IsDeleted
                            && pageContentAlias.Region.Id == regionId.Value)
                        .OrderBy(() => pageContentAlias.CreatedOn).Asc
                        .SelectList(select => select
                            .Select(() => htmlContentAlias.Html).WithAlias(() => blogModelAlias.Content)
                            .Select(() => htmlContentAlias.ActivationDate).WithAlias(() => blogModelAlias.LiveFromDate)
                            .Select(() => htmlContentAlias.ExpirationDate).WithAlias(() => blogModelAlias.LiveToDate))
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
                }
                else
                {
                    model = new BlogPostViewModel();
                }

                // Tags
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