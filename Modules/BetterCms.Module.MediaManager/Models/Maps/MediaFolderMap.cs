using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaFolderMap : EntitySubClassMapBase<MediaFolder>
    {
        public MediaFolderMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFolders");

            References(f => f.ParentFolder).Cascade.SaveUpdate().LazyLoad().Nullable();

            HasMany(f => f.Medias).KeyColumn("FolderId").Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}