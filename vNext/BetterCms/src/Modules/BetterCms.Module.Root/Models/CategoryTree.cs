using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CategoryTree : EquatableEntity<CategoryTree>, ICategoryTree
    {
        public virtual string Title { get; set; }

        public virtual IList<Category> Categories { get; set; }

        public virtual string Macro { get; set; }

        public virtual IList<CategoryTreeCategorizableItem> AvailableFor { get; set; }

        IList<ICategory> ICategoryTree.Categories
        {
            get
            {
                return Categories.Cast<ICategory>().ToList();
            }
            set
            {
                Categories = value.Cast<Category>().ToList();
            }
        }

        IList<ICategoryTreeCategorizableItem> ICategoryTree.AvailableFor
        {
            get
            {
                return AvailableFor.Cast<ICategoryTreeCategorizableItem>().ToList();
            }
            set
            {
                AvailableFor = value.Cast<CategoryTreeCategorizableItem>().ToList();
            }
        }
    }
}