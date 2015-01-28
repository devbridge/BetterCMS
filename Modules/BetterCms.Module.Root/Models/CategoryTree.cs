using System;

using BetterCms.Core.Models;
using System.Collections.Generic;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CategoryTree : EquatableEntity<CategoryTree>
    {
        public virtual string Title { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public virtual string Macro { get; set; }
    }
}