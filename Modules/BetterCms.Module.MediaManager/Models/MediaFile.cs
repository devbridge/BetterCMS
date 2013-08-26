using System;
using System.Collections.Generic;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFile : Media
    {
        public virtual string OriginalFileName { get; set; }

        public virtual string OriginalFileExtension { get; set; }

        public virtual Uri FileUri { get; set; }

        public virtual string PublicUrl { get; set; }

        public virtual long Size { get; set; }

        public virtual bool IsTemporary { get; set; }

        public virtual bool? IsUploaded { get; set; }

        public virtual bool IsCanceled { get; set; }

        public virtual IList<MediaFileAccess> AccessRules { get; set; }

        public override Media Clone()
        {
            return CopyDataTo(new MediaFile());
        }

        public override Media CopyDataTo(Media media)
        {
            var copy = (MediaFile)base.CopyDataTo(media);

            copy.OriginalFileName = OriginalFileName;
            copy.OriginalFileExtension = OriginalFileExtension;
            copy.FileUri = FileUri;
            copy.PublicUrl = PublicUrl;
            copy.Size = Size;
            copy.IsTemporary = IsTemporary;
            copy.IsUploaded = IsUploaded;
            copy.IsCanceled = IsCanceled;

            return copy;
        }
    }
}