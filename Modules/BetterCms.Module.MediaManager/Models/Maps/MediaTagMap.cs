using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaTagMap : EntityMapBase<MediaTag>
    {
        public MediaTagMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaTags");
            
            References(x => x.Media).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Tag).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
