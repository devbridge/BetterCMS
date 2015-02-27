using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;

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
            var q = repository.AsQueryable<T>().Where(ec => (CategoryTree)ec.Category.CategoryTree == categoryTree).ToList();
            var query = repository.AsQueryable<T>().Where(ec => (CategoryTree)ec.Category.CategoryTree == categoryTree).ToRowCountFutureValue();

            return query;
        }
    }
}