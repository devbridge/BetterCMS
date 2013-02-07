using System;
using System.Collections.Generic;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PageProperties : Page
    {
        public virtual string Description { get; set; }
        public virtual string CanonicalUrl { get; set; }
        public virtual string CustomCss { get; set; }
        public virtual string CustomJS { get; set; }

        public virtual bool UseCanonicalUrl { get; set; }
        public virtual bool UseNoFollow { get; set; }
        public virtual bool UseNoIndex { get; set; }

        public virtual IList<PageTag> PageTags { get; set; }
        
        public virtual Category Category { get; set; }
        public virtual MediaImage Image { get; set; }
    }
}