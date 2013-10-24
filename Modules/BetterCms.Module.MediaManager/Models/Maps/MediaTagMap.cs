using BetterCms.Core.Models;
using BetterCms.Module.MediaManager;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.Pages.Models.Maps
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
