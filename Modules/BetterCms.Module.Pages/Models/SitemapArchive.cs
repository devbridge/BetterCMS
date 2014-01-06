using System;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class SitemapArchive : EquatableEntity<SitemapArchive>
    {
        public virtual Sitemap Sitemap { get; set; }
        public virtual string ArchivedVersion { get; set; }
    }
}