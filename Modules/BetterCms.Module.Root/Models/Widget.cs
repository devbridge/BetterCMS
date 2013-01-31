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

        public override Content CopyDataTo(Content content)
        {
            var copy = (Widget)base.CopyDataTo(content);
            copy.Category = Category;

            return copy;
        }

        public override Content Clone()
        {
            return CopyDataTo(new Widget());
        }
    }
}