using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class LayoutMap : EntityMapBase<Layout>
    {
        public LayoutMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Layouts");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.LayoutPath).Length(MaxLength.Url).Not.Nullable();
            Map(x => x.PreviewUrl).Length(MaxLength.Url).Nullable();

            References(x => x.Module).Nullable().Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.Pages).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.LayoutRegions).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.LayoutOptions).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
