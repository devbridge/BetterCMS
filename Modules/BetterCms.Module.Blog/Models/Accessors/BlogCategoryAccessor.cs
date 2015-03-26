using System.Linq;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;

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
    }
}