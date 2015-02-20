using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Category : EquatableEntity<Category>, ICategory
    {
        public virtual CategoryTree CategoryTree { get; set; }

        public virtual string Name { get; set; }

        public virtual int DisplayOrder { get; set; }

        public virtual IList<Category> ChildCategories { get; set; }

        public virtual Category ParentCategory { get; set; }

        public virtual string Macro { get; set; }
    }
}