using System;
using System.Collections.Generic;

using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CategoryTree : EquatableEntity<CategoryTree>
    {
        public virtual string Title { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public virtual string Macro { get; set; }

        public virtual IList<CategoryTreeCategorizableItem> AvailableFor { get; set; }
    }
}