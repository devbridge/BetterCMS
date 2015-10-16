using System;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class LayoutRegion : EquatableEntity<LayoutRegion>
    {
        public virtual string Description { get; set; }

        public virtual Layout Layout { get; set; }

        public virtual Region Region { get; set; }
    }
}