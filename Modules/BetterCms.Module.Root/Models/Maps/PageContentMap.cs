using BetterCms.Module.Root.Models.Maps.Predefined;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentMap : PageContentEntityMapBase<PageContent>
    {
        public PageContentMap()
            : base(RootModuleDescriptor.ModuleName, "PageContents")
        {
            References(x => x.Region).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Content).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Page).Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.Options).KeyColumn("PageContentId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.History).KeyColumn("PageContentId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
