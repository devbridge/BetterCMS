using BetterCms.Core.Models;
using System.Collections.Generic;

namespace BetterCms.Module.Root.Models
{
    public class CategoryTree : EquatableEntity<CategoryTree>
    {
        public virtual string Title { get; set; }

        public virtual IList<Category> RootCategories { get; set; }
    }
}