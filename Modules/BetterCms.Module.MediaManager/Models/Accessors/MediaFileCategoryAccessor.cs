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
    public class MediaFileCategoryAccessor : ICategoryAccessor
    {
        public string Name
        {
            get
            {
                return MediaFile.CategorizableItemKeyForFiles;
            }
        }

        public IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            return repository.AsQueryable<MediaCategory>().Where(m => m.Media is MediaFile && m.Category.CategoryTree == categoryTree && m.Media.Original == null).ToRowCountFutureValue();
        }

        public IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category)
        {
            return repository.AsQueryable<MediaCategory>().Where(m =>  m.Media is MediaFile && m.Category.Id == category.Id).ToFuture();
        }
    }
}