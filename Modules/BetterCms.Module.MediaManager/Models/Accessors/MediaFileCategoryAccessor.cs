using System.Linq;

using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;

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
    }
}