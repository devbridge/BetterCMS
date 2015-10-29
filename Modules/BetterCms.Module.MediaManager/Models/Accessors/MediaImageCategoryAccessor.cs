using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.MediaManager.Models.Accessors
{
    public class MediaImageCategoryAccessor : ICategoryAccessor
    {
        public string Name
        {
            get
            {
                return MediaImage.CategorizableItemKeyForImages;
            }
        }

        public IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<MediaCategory>().Where(mc => mc.Media is MediaImage && mc.Category.CategoryTree == categoryTree && mc.Media.Original == null);
            return query.ToRowCountFutureValue();
        }

        public IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category)
        {
            return repository.AsQueryable<MediaCategory>().Where(m => m.Media is MediaImage && m.Category.Id == category.Id).ToFuture();
        }
    }
}