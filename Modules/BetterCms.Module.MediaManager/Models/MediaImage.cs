using System;

using BetterCms.Core.Security;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaImage : MediaFile, IAccessControlDisabled
    {
        public const string CategorizableItemKeyForImages = "Images";

        public virtual string Caption { get; set; }

        public virtual MediaImageAlign? ImageAlign { get; set; }

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public virtual int? CropCoordX1 { get; set; }

        public virtual int? CropCoordY1 { get; set; }

        public virtual int? CropCoordX2 { get; set; }

        public virtual int? CropCoordY2 { get; set; }

        public virtual int OriginalWidth { get; set; }

        public virtual int OriginalHeight { get; set; }

        public virtual long OriginalSize { get; set; }

        public virtual Uri OriginalUri { get; set; }

        public virtual bool? IsOriginalUploaded { get; set; }

        public virtual string PublicOriginallUrl { get; set; }

        public virtual int ThumbnailWidth { get; set; }

        public virtual int ThumbnailHeight { get; set; }

        public virtual long ThumbnailSize { get; set; }

        public virtual Uri ThumbnailUri { get; set; }

        public virtual bool? IsThumbnailUploaded { get; set; }

        public virtual string PublicThumbnailUrl { get; set; }

        public override string GetCategorizableItemKey()
        {
            return CategorizableItemKeyForImages;
        }

        public override Media Clone()
        {
            return CopyDataTo(new MediaImage());
        }

        public override Media CopyDataTo(Media media, bool copyCollections = true)
        {
            var copy = (MediaImage)base.CopyDataTo(media, copyCollections);

            copy.Caption = Caption;
            copy.ImageAlign = ImageAlign;
            copy.Width = Width;
            copy.Height = Height;
            copy.CropCoordX1 = CropCoordX1;
            copy.CropCoordY1 = CropCoordY1;
            copy.CropCoordX2 = CropCoordX2;
            copy.CropCoordY2 = CropCoordY2;
            copy.OriginalWidth = OriginalWidth;
            copy.OriginalHeight = OriginalHeight;
            copy.OriginalSize = OriginalSize;
            copy.OriginalUri = OriginalUri;
            copy.IsOriginalUploaded = IsOriginalUploaded;
            copy.PublicOriginallUrl = PublicOriginallUrl;
            copy.ThumbnailWidth = ThumbnailWidth;
            copy.ThumbnailHeight = ThumbnailHeight;
            copy.ThumbnailSize = ThumbnailSize;
            copy.ThumbnailUri = ThumbnailUri;
            copy.IsThumbnailUploaded = IsThumbnailUploaded;
            copy.PublicThumbnailUrl = PublicThumbnailUrl;

            return copy;
        }

        public virtual bool IsEdited()
        {
            return CropCoordX1 != null && CropCoordX2 != null && CropCoordY1 != null && CropCoordY2 != null
                || Height != OriginalHeight || Width != OriginalWidth;
        }
    }
}