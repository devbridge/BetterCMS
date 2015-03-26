using System.Linq;

using BetterCms.Core.DataContracts;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;

namespace BetterCms.Module.Root.Models.Accessors
{
    public class DefaultCategoryAccessor<T> : ICategoryAccessor
        where T : class, IEntityCategory
    {
        private readonly string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public DefaultCategoryAccessor(string name)
        {
            this.name = name;
        } 
        public virtual IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<T>().Where(ec => (CategoryTree)ec.Category.CategoryTree == categoryTree).ToRowCountFutureValue();
            return query;
        }
    }
}