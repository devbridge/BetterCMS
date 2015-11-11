using System.Collections.Generic;

using BetterCms.Core.DataContracts;

using BetterModules.Core.DataAccess;

using NHibernate;

namespace BetterCms.Module.Root.Models
{
    public interface ICategoryAccessor
    {
        string Name { get; }
        IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree);
        IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category);
    }
}