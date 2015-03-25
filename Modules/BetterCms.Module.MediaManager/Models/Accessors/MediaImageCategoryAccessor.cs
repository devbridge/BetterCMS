using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models;

using NHibernate;

namespace BetterCms.Module.MediaManager.Models.Accessors
{
    public class MediaImageCategoryAccessor :  ICategoryAccessor
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
    }
}