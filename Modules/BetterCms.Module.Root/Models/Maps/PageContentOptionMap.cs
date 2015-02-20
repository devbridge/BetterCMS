using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageContentOptionMap : EntityMapBase<PageContentOption>
    {
        public PageContentOptionMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("PageContentOptions");

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.Value).Length(MaxLength.Max).Nullable();         
            
            References(x => x.PageContent).Cascade.SaveUpdate().LazyLoad();
            References(x => x.CustomOption).Cascade.SaveUpdate().LazyLoad();
        }
    }
}