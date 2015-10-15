using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ChildContentOptionMap : EntityMapBase<ChildContentOption>
    {
        public ChildContentOptionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("ChildContentOptions");

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.Value).Length(MaxLength.Max).Nullable();

            References(x => x.ChildContent).Cascade.SaveUpdate().LazyLoad();
            References(x => x.CustomOption).Cascade.SaveUpdate().LazyLoad();
            HasMany(x => x.Translations).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}