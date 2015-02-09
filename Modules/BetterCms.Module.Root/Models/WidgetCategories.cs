using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

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