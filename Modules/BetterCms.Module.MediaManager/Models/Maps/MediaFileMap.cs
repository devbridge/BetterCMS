using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaFileMap : EntitySubClassMapBase<MediaFile>
    {
        public MediaFileMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFiles");            

            Map(f => f.FileName).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.FileExtension).Nullable().Length(MaxLength.Name);
            Map(f => f.FileUri).Not.Nullable().Length(MaxLength.Uri).LazyLoad();
            Map(f => f.PublicUrl).Not.Nullable().Length(MaxLength.Url).LazyLoad();
            Map(f => f.Size).Not.Nullable();
            Map(f => f.IsTemporary).Not.Nullable().Default("1");
            Map(f => f.IsStored).Not.Nullable().Default("0");
        }
    }
}