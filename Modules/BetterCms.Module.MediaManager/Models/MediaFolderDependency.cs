using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFolderDependency : EquatableEntity<MediaFolderDependency>
    {
        public virtual MediaFolder Parent { get; set; }
        
        public virtual MediaFolder Child { get; set; }

        public override string ToString()
        {
            return string.Format("MediaFolderDependency: {0}", base.ToString());
        }
    }
}