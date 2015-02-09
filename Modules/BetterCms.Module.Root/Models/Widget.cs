using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Widget : Content, IWidget, ICategorized
    {
        public virtual IList<WidgetCategory> Categories { get; set; }

        IEnumerable<IEntityCategory> ICategorized.Categories
        {
            get
            {
                return Categories;
            }
        }

        public override Content CopyDataTo(Content content, bool copyCollections = true)
        {
            var copy = (Widget)base.CopyDataTo(content, copyCollections);
            copy.Categories = Categories;

            return copy;
        }

        public override Content Clone()
        {
            return CopyDataTo(new Widget());
        }


        public virtual void AddCategory(IEntityCategory category)
        {
            if (Categories == null)
            {
                Categories = new List<WidgetCategory>();
            }

            Categories.Add(category as WidgetCategory);
        }

        public virtual void RemoveCategory(IEntityCategory category)
        {
            if (Categories != null)
            {
                Categories.Remove(category as WidgetCategory);
            }
        }
    }
}