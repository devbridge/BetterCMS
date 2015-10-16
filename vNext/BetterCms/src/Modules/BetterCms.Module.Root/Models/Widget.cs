using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Widget : Content, IWidget, ICategorized
    {
        public virtual IList<WidgetCategory> Categories { get; set; }
        public const string CategorizableItemKeyForWidgets = "Widgets";


        IEnumerable<IEntityCategory> ICategorized.Categories
        {
            get
            {
                return Categories;
            }
        }

        public virtual string GetCategorizableItemKey()
        {
            return CategorizableItemKeyForWidgets;
        }

        public override Content CopyDataTo(Content content, bool copyCollections = true)
        {
            var copy = (Widget)base.CopyDataTo(content, copyCollections);

            if (copyCollections && Categories != null)
            {
                if (copy.Categories == null)
                {
                    copy.Categories = new List<WidgetCategory>();
                }

                foreach (var category in Categories)
                {
                    var clonedWidget = category.Clone();
                    clonedWidget.Widget = copy;

                    copy.Categories.Add(clonedWidget);
                }
            }

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