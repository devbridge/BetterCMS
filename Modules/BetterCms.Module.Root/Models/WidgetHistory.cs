using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class WidgetHistory : ContentHistory, IWidget
    {
        public virtual Category Category { get; set; }

        ICategory IWidget.Category
        {
            get
            {
                return Category;
            }
        }
    }
}