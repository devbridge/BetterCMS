using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

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