using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.ViewModels;
using BetterCms.Module.Root.ViewModels.Category;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Root.Commands.Category.GetCategoryTreesList
{
    public class GetCategoryTreesListCommand : CommandBase, ICommand<CategoryTreesFilter, CategoryTreesGridViewModel<SiteSettingCategoryTreeViewModel>>
    {
        public CategoryTreesGridViewModel<SiteSettingCategoryTreeViewModel> Execute(CategoryTreesFilter request)
        {
            request.SetDefaultSortingOptions("Title");

            CategoryTree alias = null;
            SiteSettingCategoryTreeViewModel modelAlias = null;
            
            var query = UnitOfWork.Session.QueryOver(() => alias).Where(() => !alias.IsDeleted);
            
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchQuery = string.Format("%{0}%", request.SearchQuery);
                query = query.Where(Restrictions.Disjunction().Add(Restrictions.InsensitiveLike(NHibernate.Criterion.Projections.Property(() => alias.Title), searchQuery)));
            }
            
            query =
                query.SelectList(
                    select =>
                    select.Select(() => alias.Id)
                            .WithAlias(() => modelAlias.Id)
                            .Select(() => alias.Version)
                            .WithAlias(() => modelAlias.Version)
                            .Select(() => alias.Title)
                            .WithAlias(() => modelAlias.Title)).TransformUsing(Transformers.AliasToBean<SiteSettingCategoryTreeViewModel>());
            
            var count = query.ToRowCountFutureValue();
            
            var sitemaps = query.AddSortingAndPaging(request).Future<SiteSettingCategoryTreeViewModel>();

            return new CategoryTreesGridViewModel<SiteSettingCategoryTreeViewModel>(sitemaps.ToList(), request, count.Value);
        }
    }
}