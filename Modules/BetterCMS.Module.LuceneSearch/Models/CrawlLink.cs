using System;

namespace BetterCMS.Module.LuceneSearch.Models
{
    public class CrawlLink
    {
        public Guid Id { get; set; }
        
        public string Path { get; set; }
        
        public bool IsPublished { get; set; }
    }
}
