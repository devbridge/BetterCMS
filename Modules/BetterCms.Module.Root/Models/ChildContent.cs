using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ChildContent : EquatableEntity<ChildContent>
    {
        public virtual Content Parent { get; set; }

        public virtual Content Child { get; set; }
    }
}