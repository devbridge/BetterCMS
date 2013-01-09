using BetterCms.Module.Root.Models.Maps.Predefined;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentOptionHistoryMap : PageContentOptionEntityMapBase<PageContentOptionHistory>
    {
        public PageContentOptionHistoryMap()
            : base(RootModuleDescriptor.ModuleName, "PageContentOptionHistory")
        {
            References(x => x.ContentOption).Cascade.SaveUpdate().LazyLoad();
            References(x => x.PageContent).Cascade.SaveUpdate().LazyLoad();
        }
    }
}