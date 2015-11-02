using System;

namespace BetterCMS.Module.LuceneSearch.Services.IndexerService
{
    public class DocumentData
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsPublished { get; set; }
    }
}
