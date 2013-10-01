using BetterCms.Core.Models;

namespace BetterCms.Module.ImagesGallery.Models.Maps
{
    public class AlbumMap : EntityMapBase<Album>
    {
        public AlbumMap()
            : base(ImagesGalleryModuleDescriptor.ModuleName)
        {
            Table("Albums");

            Map(f => f.Title).Not.Nullable().Length(MaxLength.Name);
            
            References(f => f.Folder).Cascade.SaveUpdate().LazyLoad().Nullable();
            References(f => f.CoverImage).Cascade.SaveUpdate().LazyLoad().Nullable();
        }
    }
}