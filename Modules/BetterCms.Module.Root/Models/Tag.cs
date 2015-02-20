using System;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Tag : EquatableEntity<Tag>
    {
        public virtual string Name { get; set; }
    }
}