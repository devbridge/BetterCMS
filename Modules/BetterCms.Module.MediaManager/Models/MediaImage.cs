using System;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaImage : MediaFile
    {
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

        public virtual bool IsOriginalUploaded { get; set; }

        public virtual int ThumbnailWidth { get; set; }

        public virtual int ThumbnailHeight { get; set; }

        public virtual long ThumbnailSize { get; set; }

        public virtual Uri ThumbnailUri { get; set; }

        public virtual bool IsThumbnailUploaded { get; set; }
    }
}