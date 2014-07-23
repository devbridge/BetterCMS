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

        public override Content CopyDataTo(Content content, bool copyCollections = true)
        {
            var copy = (Widget)base.CopyDataTo(content, copyCollections);
            copy.Category = Category;

            return copy;
        }

        public override Content Clone()
        {
            return CopyDataTo(new Widget());
        }
    }
}