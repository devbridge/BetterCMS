using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentRegion : EquatableEntity<ContentRegion>
    {
        public virtual Content Content { get; set; }

        public virtual Region Region { get; set; }
    }
}