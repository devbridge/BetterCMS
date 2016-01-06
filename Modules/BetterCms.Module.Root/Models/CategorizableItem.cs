using System;
using BetterCms.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CategorizableItem : EquatableEntity<CategorizableItem> , ICategorizableItem
    {
        public virtual string Name { get; set; }
    }
}