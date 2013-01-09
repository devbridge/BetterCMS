using BetterCms.Module.Root.Models.Maps.Predefined;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentHistoryMap : PageContentEntityMapBase<PageContentHistory>
    {
        public PageContentHistoryMap()
            : base(RootModuleDescriptor.ModuleName, "PageContentHistory")
        {
            References(x => x.PageContent).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Region).Cascade.SaveUpdate().LazyLoad();
            References(x => x.ContentHistory).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Page).Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.PageContentOptionHistory).Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
