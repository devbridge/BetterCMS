using System;

namespace BetterCms.Module.Installation.Models.Blog
{
    public class CategoryItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Id: {1}", Name, Id);
        }
    }
}