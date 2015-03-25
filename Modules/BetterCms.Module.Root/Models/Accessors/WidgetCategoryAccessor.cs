using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using NHibernate;

namespace BetterCms.Module.Root.Models.Accessors
{
    public class WidgetCategoryAccessor : ICategoryAccessor
    {
        public string Name
        {
            get
            {
                return Widget.CategorizableItemKeyForWidgets;
            } 
        }

        public IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<WidgetCategory>().Where(ec => ec.Category.CategoryTree == categoryTree
            && !ec.Widget.IsDeleted
            && (ec.Widget.Status == ContentStatus.Draft || ec.Widget.Status == ContentStatus.Published)).ToRowCountFutureValue();

            return query;
        }
    }
}