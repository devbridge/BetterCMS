using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentOptionMap : EntityMapBase<PageContentOption>
    {
        public PageContentOptionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("PageContentOptions");

            Map(x => x.Value).Length(MaxLength.Text).Nullable().LazyLoad();

            References(x => x.ContentOption).Cascade.SaveUpdate().LazyLoad();
            References(x => x.PageContent).Cascade.SaveUpdate().LazyLoad();
        }
    }
}