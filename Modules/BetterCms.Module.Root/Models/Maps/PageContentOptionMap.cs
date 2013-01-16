using BetterCms.Module.Root.Models.Maps.Predefined;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentOptionMap : PageContentOptionEntityMapBase<PageContentOption>
    {
        public PageContentOptionMap()
            : base(RootModuleDescriptor.ModuleName, "PageContentOptions")
        {
            References(x => x.ContentOption).Cascade.SaveUpdate().LazyLoad();
            References(x => x.PageContent).Cascade.SaveUpdate().LazyLoad();
        }
    }
}