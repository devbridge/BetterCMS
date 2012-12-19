using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class LayoutRegionMap : EntityMapBase<LayoutRegion>
    {
        public LayoutRegionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("LayoutRegions");

            References(f => f.Layout).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Region).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}
