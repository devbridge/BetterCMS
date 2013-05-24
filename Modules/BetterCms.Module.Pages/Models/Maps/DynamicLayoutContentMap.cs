using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class DynamicLayoutContentMap : EntitySubClassMapBase<DynamicLayoutContent>
    {
        public DynamicLayoutContentMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("DynamicLayoutContents");

            References(x => x.Layout).Not.Nullable().Cascade.SaveUpdate().LazyLoad();
        }
    }
}
