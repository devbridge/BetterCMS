using System;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFile : Media
    {
        public virtual string FileName { get; set; }

        public virtual string FileExtension { get; set; }

        public virtual Uri FileUri { get; set; }

        public virtual string PublicUrl { get; set; }

        public virtual long Size { get; set; }

        public virtual bool IsTemporary { get; set; }

        public virtual bool IsStored { get; set; }
    }
}