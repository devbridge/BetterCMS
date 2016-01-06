using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Blog.Models.Accessors
{
    public class BlogCategoryAccessor : ICategoryAccessor
    {
        public string Name
        {
            get
            {
                return BlogPost.CategorizableItemKeyForBlogs;
            }
        }

        public IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<PageCategory>().Where(p => p.Page is BlogPost && p.Category.CategoryTree == categoryTree);
            return query.ToRowCountFutureValue();
        }

        public IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category)
        {
            return repository.AsQueryable<PageCategory>().Where(p => p.Page is BlogPost && p.Category.Id == category.Id).ToFuture();
        }
    }
}