using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaFolderMap : EntitySubClassMapBase<MediaFolder>
    {
        public MediaFolderMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFolders");

            HasMany(f => f.Medias).KeyColumn("FolderId").Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(f => f.ChildFolders).KeyColumn("ParentId").Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(f => f.ParentFolders).KeyColumn("ChildId").Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}