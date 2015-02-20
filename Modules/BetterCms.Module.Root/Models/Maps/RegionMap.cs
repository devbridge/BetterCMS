using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class RegionMap : EntityMapBase<Region>
    {
        public RegionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Regions");
            
            Map(x => x.RegionIdentifier).Not.Nullable().Length(MaxLength.Name);

            HasMany(x => x.PageContents).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");

            HasMany(x => x.LayoutRegion).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
