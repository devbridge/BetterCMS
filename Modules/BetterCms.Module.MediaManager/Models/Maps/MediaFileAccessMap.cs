using BetterCms.Core.Models;
using BetterCms.Module.Root;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class PageAccessMap : EntityMapBase<MediaManager.Models.MediaFileAccess>
    {
        public PageAccessMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFileAccess");

            References(x => x.MediaFile).Cascade.SaveUpdate().Not.Nullable().LazyLoad();

            Map(x => x.RoleOrUser).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.AccessLevel).Not.Nullable();
        }
    }
}