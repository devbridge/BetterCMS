using System;

using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaTag : EquatableEntity<MediaTag>, IMediaProvider 
    {
        public virtual Tag Tag { get; set; }
        public virtual Media Media { get; set; }

        public virtual MediaTag Clone()
        {
            return CopyDataTo(new MediaTag());
        }

        public virtual MediaTag CopyDataTo(MediaTag mediaTag)
        {
            mediaTag.Tag = Tag;
            mediaTag.Media = Media;

            return mediaTag;
        }
    }
}