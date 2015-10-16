using System;
using System.Collections.Generic;
using System.Linq;

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

        ICategoryTree ICategory.CategoryTree
        {
            get
            {
                return CategoryTree;
            }
            set
            {
                CategoryTree = value as CategoryTree;
            }
        }

        ICategory ICategory.ParentCategory
        {
            get
            {
                return ParentCategory;
            }
            set
            {
                ParentCategory = value as Category;
            }
        }

        IList<ICategory> ICategory.ChildCategories
        {
            get
            {
                return ChildCategories.Cast<ICategory>().ToList();
            }
            set
            {
                ChildCategories = value.Cast<Category>().ToList();
            }
        } 

    }
}