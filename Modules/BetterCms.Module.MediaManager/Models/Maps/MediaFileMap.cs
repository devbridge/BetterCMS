using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaFileMap : EntitySubClassMapBase<MediaFile>
    {
        public MediaFileMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFiles");            

            Map(f => f.OriginalFileName).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.OriginalFileExtension).Nullable().Length(MaxLength.Name);
            Map(f => f.FileUri).Not.Nullable().Length(MaxLength.Uri);
            Map(f => f.PublicUrl).Not.Nullable().Length(MaxLength.Url);
            Map(f => f.Size).Not.Nullable();
            Map(f => f.IsTemporary).Not.Nullable().Default("1");
            Map(f => f.IsUploaded).Nullable();
            Map(f => f.IsCanceled).Not.Nullable().Default("0");

            HasManyToMany(x => x.AccessRules).Table("MediaFileAccessRules").Schema(SchemaName).Cascade.AllDeleteOrphan().LazyLoad();
        }
    }
}