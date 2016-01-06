using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Blog.ViewModels.Filter;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Transform;

namespace BetterCms.Module.Blog.Commands.GetBlogPostList
{
    /// <summary>
    /// A command for get blogs list by filter.
    /// </summary>
    public class GetBlogPostListCommand : CommandBase, ICommand<BlogsFilter, BlogsGridViewModel<SiteSettingBlogPostViewModel>>
    {
        /// <summary>
        /// The category service
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The languages service
        /// </summary>
        private readonly ILanguageService languageService;

        /// <summary>
        /// The blog service
        /// </summary>
        private readonly IBlogService blogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostListCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="languageService">The language service.</param>
        /// <param name="blogService">The blog service.</param>
        public GetBlogPostListCommand(ICategoryService categoryService, ICmsConfiguration configuration,
            ILanguageService languageService, IBlogService blogService)
        {
            this.categoryService = categoryService;
            this.configuration = configuration;
            this.languageService = languageService;
            this.blogService = blogService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A list of blog posts</returns>
        public BlogsGridViewModel<SiteSettingBlogPostViewModel> Execute(BlogsFilter request)
        {
            BlogPost alias = null;
            SiteSettingBlogPostViewModel modelAlias = null;

            var query = blogService.GetFilteredBlogPostsQuery(request);

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Title)
                    .Select(() => alias.CreatedOn).WithAlias(() => modelAlias.CreatedOn)
                    .Select(() => alias.ModifiedOn).WithAlias(() => modelAlias.ModifiedOn)
                    .Select(() => alias.ModifiedByUser).WithAlias(() => modelAlias.ModifiedByUser)
                    .Select(() => alias.Status).WithAlias(() => modelAlias.Status)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.PageUrl).WithAlias(() => modelAlias.PageUrl))
                .TransformUsing(Transformers.AliasToBean<SiteSettingBlogPostViewModel>());

            var count = query.ToRowCountFutureValue();

            var blogPosts = query.AddSortingAndPaging(request).Future<SiteSettingBlogPostViewModel>();
            IEnumerable<LookupKeyValue> languagesFuture = configuration.EnableMultilanguage ? languageService.GetLanguagesLookupValues() : null;

            var model = new BlogsGridViewModel<SiteSettingBlogPostViewModel>(blogPosts.ToList(), request, count.Value);
            if (languagesFuture != null)
            {
                model.Languages = languagesFuture.ToList();
                model.Languages.Insert(0, languageService.GetInvariantLanguageModel());
            }

            return model;
        }
    }
}