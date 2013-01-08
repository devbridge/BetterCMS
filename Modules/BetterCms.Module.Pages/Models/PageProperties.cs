using System;
using System.Collections.Generic;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PageProperties : Page
    {
        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string CanonicalUrl { get; set; }
        public virtual string CustomCss { get; set; }

        public virtual bool UseCanonicalUrl { get; set; }
        public virtual bool UseCustomCss { get; set; }
        public virtual bool UseNoFollow { get; set; }
        public virtual bool UseNoIndex { get; set; }
        public virtual bool IsPublic { get; set; }

        public virtual IList<PageTag> PageTags { get; set; }
        public virtual IList<PageCategory> PageCategories { get; set; }
        
        public virtual Author Author { get; set; }
        public virtual Category Category { get; set; }
    }
}