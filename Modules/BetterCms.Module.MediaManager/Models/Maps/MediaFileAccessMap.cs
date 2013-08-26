using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class PageAccessMap : EntityMapBase<MediaFileAccess>
    {
        public PageAccessMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFileAccess");

            References(x => x.MediaFile).Cascade.SaveUpdate().Not.Nullable().LazyLoad();

            Map(x => x.RoleOrUser).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.AccessLevel).Not.Nullable();

            Map(x => x.ObjectId).Column("MediaFileId").Not.Nullable().ReadOnly();
        }
    }
}