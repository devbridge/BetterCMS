using System;

using BetterCms.Core.DataContracts;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class WidgetCategory : EquatableEntity<WidgetCategory>, IEntityCategory
    {
        public virtual Category Category { get; set; }


        public virtual void SetEntity(IEntity entity)
        {
            Widget = entity as Widget;
        }

        public virtual Widget Widget { get; set; }

        ICategory IEntityCategory.Category
        {
            get
            {
                return Category;
            }
            set
            {
                Category = value as Category;
            }
        }

        IEntity IEntityCategory.Entity
        {
            get
            {
                return Widget;
            }
            set
            {
                Widget = value as Widget;
            }
        }

        public virtual WidgetCategory Clone()
        {
            return CopyDataTo(new WidgetCategory());
        }

        public virtual WidgetCategory CopyDataTo(WidgetCategory widget)
        {
            widget.Widget = Widget;
            widget.Category = Category;

            return widget;
        }
    }
}