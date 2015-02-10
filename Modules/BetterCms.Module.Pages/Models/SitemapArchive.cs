using System;

using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class SitemapArchive : EquatableEntity<SitemapArchive>
    {
        public virtual Sitemap Sitemap { get; set; }
        public virtual string Title { get; set; }
        public virtual string ArchivedVersion { get; set; }
    }
}