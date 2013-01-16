using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Widget : Content, IWidget
    {
        public virtual Category Category { get; set; }

        ICategory IWidget.Category
        {
            get
            {
                return Category;
            }
        }

        public override Content Clone()
        {
            return new Widget
                {
                    Name = Name,
                    Category = Category
                };
        }
    }
}