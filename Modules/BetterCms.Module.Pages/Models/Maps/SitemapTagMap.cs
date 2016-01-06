using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class SitemapTagMap : EntityMapBase<SitemapTag>
    {
        public SitemapTagMap() : base(PagesModuleDescriptor.ModuleName)
        {
            Table("SitemapTags");

            References(x => x.Sitemap).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Tag).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
