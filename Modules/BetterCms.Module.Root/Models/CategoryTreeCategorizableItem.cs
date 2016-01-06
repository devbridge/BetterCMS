using System;
using BetterCms.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CategoryTreeCategorizableItem : EquatableEntity<CategoryTreeCategorizableItem>, ICategoryTreeCategorizableItem
    {
        public virtual CategoryTree CategoryTree { get; set; }
        public virtual CategorizableItem CategorizableItem { get; set; }

        ICategoryTree ICategoryTreeCategorizableItem.CategoryTree
        {
            get
            {
                return CategoryTree;
            }
            set
            {
                CategoryTree = (CategoryTree)value;
            }
        }

        ICategorizableItem ICategoryTreeCategorizableItem.CategorizableItem
        {
            get
            {
                return CategorizableItem;
            }
            set
            {
                CategorizableItem = (CategorizableItem)value;
            }
        }
    }
}