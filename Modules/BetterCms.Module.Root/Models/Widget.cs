using System;

using BetterCms.Core.DataContracts;

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

        public override Content CopyDataTo(Content content, bool copyOptions = true, bool copyRegions = true)
        {
            var copy = (Widget)base.CopyDataTo(content, copyOptions, copyRegions);
            copy.Category = Category;

            return copy;
        }

        public override Content Clone()
        {
            return CopyDataTo(new Widget());
        }
    }
}