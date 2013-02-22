using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Blog.Commands.GetBlogPostList
{
    /// <summary>
    /// A command for get blogs list by filter.
    /// </summary>
    public class GetBlogPostListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<SiteSettingBlogPostViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A list of blog posts</returns>
        public SearchableGridViewModel<SiteSettingBlogPostViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<SiteSettingBlogPostViewModel> model;

            request.SetDefaultSortingOptions("Title");

            BlogPost alias = null;
            SiteSettingBlogPostViewModel modelAlias = null;

            var query = UnitOfWork.Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted && alias.Status != PageStatus.Preview);

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.InsensitiveLike(Projections.Property(() => alias.Title), searchQuery));
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

            model = new SearchableGridViewModel<SiteSettingBlogPostViewModel>(blogPosts.ToList(), request, count.Value);

            return model;
        }
    }
}