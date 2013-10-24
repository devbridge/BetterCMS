using System;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaTag : EquatableEntity<MediaTag>
    {
        public virtual Tag Tag { get; set; }
        public virtual Media Media { get; set; }
    }
}