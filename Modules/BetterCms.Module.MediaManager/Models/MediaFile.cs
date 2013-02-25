using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFile : Media, IMediaFile
    {
        public virtual string OriginalFileName { get; set; }

        public virtual string OriginalFileExtension { get; set; }

        public virtual Uri FileUri { get; set; }

        public virtual string PublicUrl { get; set; }

        public virtual long Size { get; set; }

        public virtual bool IsTemporary { get; set; }

        public virtual bool IsUploaded { get; set; }

        public virtual bool IsCanceled { get; set; }
    }
}