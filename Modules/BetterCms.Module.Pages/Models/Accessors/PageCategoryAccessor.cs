using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models;

using NHibernate;

namespace BetterCms.Module.Pages.Models.Accessors
{
    public class PageCategoryAccessor : ICategoryAccessor
    {

        public string Name
        {
            get
            {
                return PageProperties.CategorizableItemKeyForPages;
            }
        }

        public IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<PageCategory>().Where(p => p.Page is PageProperties && p.Category.CategoryTree == categoryTree);
            return query.ToRowCountFutureValue();
        }
    }
}