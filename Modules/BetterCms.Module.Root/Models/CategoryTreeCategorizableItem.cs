using System;

using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CategoryTreeCategorizableItem : EquatableEntity<CategoryTreeCategorizableItem>
    {
        public virtual CategoryTree CategoryTree { get; set; }
        public virtual CategorizableItem CategorizableItem { get; set; }
    }
}