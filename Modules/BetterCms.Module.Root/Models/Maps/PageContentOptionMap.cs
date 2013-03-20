using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentOptionMap : EntityMapBase<PageContentOption>
    {
        public PageContentOptionMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("PageContentOptions");

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.Value).Length(MaxLength.Max).Nullable().LazyLoad();         
            
            References(x => x.PageContent).Cascade.SaveUpdate().LazyLoad();
        }
    }
}