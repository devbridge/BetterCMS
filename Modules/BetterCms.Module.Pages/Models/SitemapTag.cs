using System;

using BetterCms.Module.Root.Models;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class SitemapTag : EquatableEntity<SitemapTag>
    {
        public virtual Tag Tag { get; set; }
        public virtual Sitemap Sitemap { get; set; }
    }
}