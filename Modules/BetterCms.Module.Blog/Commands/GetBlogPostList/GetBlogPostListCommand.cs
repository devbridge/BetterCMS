using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Blog.ViewModels.Filter;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Services;

using NHibernate.Criterion;
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
        /// Initializes a new instance of the <see cref="GetBlogPostListCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="languageService">The language service.</param>
        public GetBlogPostListCommand(ICategoryService categoryService, ICmsConfiguration configuration, ILanguageService languageService)
        {
            this.categoryService = categoryService;
            this.configuration = configuration;
            this.languageService = languageService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A list of blog posts</returns>
        public BlogsGridViewModel<SiteSettingBlogPostViewModel> Execute(BlogsFilter request)
        {
            request.SetDefaultSortingOptions("Title");

            BlogPost alias = null;
            SiteSettingBlogPostViewModel modelAlias = null;

            var query = UnitOfWork.Session
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

            if (request.CategoryId.HasValue)
            {
                query = query.Where(Restrictions.Eq(Projections.Property(() => alias.Category.Id), request.CategoryId.Value));
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

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Title)
                    .Select(() => alias.CreatedOn).WithAlias(() => modelAlias.CreatedOn)
                    .Select(() => alias.ModifiedOn).WithAlias(() => modelAlias.ModifiedOn)
                    .Select(() => alias.ModifiedByUser).WithAlias(() => modelAlias.ModifiedByUser)
                    .Select(() => alias.Status).WithAlias(() => modelAlias.PageStatus)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.PageUrl).WithAlias(() => modelAlias.PageUrl))
                .TransformUsing(Transformers.AliasToBean<SiteSettingBlogPostViewModel>());

            var count = query.ToRowCountFutureValue();

            var blogPosts = query.AddSortingAndPaging(request).Future<SiteSettingBlogPostViewModel>();
            IEnumerable<LookupKeyValue> languagesFuture = configuration.EnableMultilanguage ? languageService.GetLanguages() : null;

            var model = new BlogsGridViewModel<SiteSettingBlogPostViewModel>(blogPosts.ToList(), request, count.Value, categoryService.GetCategories());
            if (languagesFuture != null)
            {
                model.Languages = languagesFuture.ToList();
                model.Languages.Insert(0, languageService.GetInvariantLanguageModel());
            }

            return model;
        }
    }
}