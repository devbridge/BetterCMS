using System.Collections.Generic;

namespace BetterCms.Module.Installation.Models.Blog
{
    public class BlogItemsModel
    {
        public IList<BlogItem> Items { get; set; }

        public bool ShowAuthor { get; set; }

        public bool ShowDate { get; set; }

        public bool ShowTags { get; set; }

        public bool ShowCategories { get; set; }

        public bool ShowPager { get; set; }

        public int NumberOfPages { get; set; }

        public int CurrentPage { get; set; }
    }
}