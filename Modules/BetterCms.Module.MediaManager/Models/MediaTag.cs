using System;

using BetterCms.Module.Root.Models;

using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaTag : EquatableEntity<MediaTag>
    {
        public virtual Tag Tag { get; set; }
        public virtual Media Media { get; set; }
    }
}