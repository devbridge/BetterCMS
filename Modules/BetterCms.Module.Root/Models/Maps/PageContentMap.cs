using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentMap : EntityMapBase<PageContent>
    {
        public PageContentMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("PageContents");

            Map(x => x.Order, "[Order]").Not.Nullable();

            References(x => x.Region).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Content).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Page).Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.Options).KeyColumn("PageContentId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
