using System;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Widget : Content
    {
        public virtual Category Category { get; set; }

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