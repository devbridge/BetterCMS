using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Setting: Entity
    {
        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual Guid ModuleId { get; set; }
    }
}