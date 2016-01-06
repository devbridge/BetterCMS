using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaCategoryMap : EntityMapBase<MediaCategory>
    {
        public MediaCategoryMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaCategories");

            References(x => x.Media).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Category).Cascade.SaveUpdate().LazyLoad();
        }
    }
}