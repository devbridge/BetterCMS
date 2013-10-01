using System;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.ImagesGallery.Models
{
    [Serializable]
    public class Album : EquatableEntity<Album>
    {
        public virtual string Title { get; set; }

        public virtual MediaFolder Folder { get; set; }

        public virtual MediaImage CoverImage { get; set; }
    }
}