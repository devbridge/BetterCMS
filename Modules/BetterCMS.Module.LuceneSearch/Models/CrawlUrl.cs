using System;

using BetterCms.Core.Models;

namespace BetterCMS.Module.LuceneSearch.Models
{
    public class CrawlUrl : EquatableEntity<CrawlUrl>
    {
        public virtual string Path { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
    }
}
