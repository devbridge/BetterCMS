using BetterCms.Core.Models;
using BetterCms.Module.LuceneSearch;

namespace BetterCMS.Module.LuceneSearch.Models.Maps
{
    class CrawlUrlMap : EntityMapBase<CrawlUrl>
    {
        public CrawlUrlMap()
            : base(LuceneSearchModuleDescriptor.ModuleName)
        {
            Table("IndexedPages");

            Map(x => x.Path).Not.Nullable();
            Map(x => x.StartTime).Nullable();
            Map(x => x.EndTime).Nullable();
        }
    }
}
