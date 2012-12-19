using System;
using System.Collections.Generic;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFolder : Media
    {
        public virtual MediaFolder ParentFolder { get; set; }

        public virtual IList<Media> Medias { get; set; }
    }
}