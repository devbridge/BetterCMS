using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaImageMap : EntitySubClassMapBase<MediaImage>
    {
        public MediaImageMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaImages");

            Map(f => f.Caption).Nullable().Length(MaxLength.Text);
            Map(f => f.ImageAlign).Nullable();
            Map(f => f.Width).Not.Nullable();
            Map(f => f.Height).Not.Nullable();
            Map(f => f.CropCoordX1).Nullable();
            Map(f => f.CropCoordY1).Nullable();
            Map(f => f.CropCoordX2).Nullable();
            Map(f => f.CropCoordY2).Nullable();
            Map(f => f.OriginalWidth).Not.Nullable();
            Map(f => f.OriginalHeight).Not.Nullable();
            Map(f => f.OriginalSize).Not.Nullable();
            Map(f => f.OriginalUri).Not.Nullable();
            Map(f => f.IsOriginalUploaded).Nullable();
            Map(f => f.PublicOriginallUrl).Not.Nullable();
            Map(f => f.ThumbnailWidth).Not.Nullable();
            Map(f => f.ThumbnailHeight).Not.Nullable();
            Map(f => f.ThumbnailSize).Not.Nullable();
            Map(f => f.ThumbnailUri).Not.Nullable();
            Map(f => f.IsThumbnailUploaded).Nullable();
            Map(f => f.PublicThumbnailUrl);
        }
    }
}