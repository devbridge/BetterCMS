using System;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class SitemapTag : EquatableEntity<SitemapTag>
    {
        public virtual Tag Tag { get; set; }
        public virtual Sitemap Sitemap { get; set; }
    }
}