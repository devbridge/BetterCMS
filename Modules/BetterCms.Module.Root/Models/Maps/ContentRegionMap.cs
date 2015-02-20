using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentRegionMap : EntityMapBase<ContentRegion>
    {
        public ContentRegionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("ContentRegions");

            References(f => f.Content).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Region).Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}
