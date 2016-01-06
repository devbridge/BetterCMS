using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;
using NHibernate.Linq;

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

        public IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category)
        {
            return repository.AsQueryable<WidgetCategory>().Where(wc => wc.Category.Id == category.Id).ToFuture();
        }
    }
}