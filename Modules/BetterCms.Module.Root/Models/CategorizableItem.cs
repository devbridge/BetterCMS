using System;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CategorizableItem : EquatableEntity<CategorizableItem>
    {
        public virtual string Name { get; set; }
    }
}