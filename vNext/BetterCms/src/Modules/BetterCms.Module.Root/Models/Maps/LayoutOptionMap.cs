using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class LayoutOptionMap : EntityMapBase<LayoutOption>
    {
        public LayoutOptionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("LayoutOptions");

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.DefaultValue).Length(MaxLength.Max).Nullable();
            Map(x => x.IsDeletable).Not.Nullable();

            References(x => x.Layout).Cascade.SaveUpdate().LazyLoad();
            References(x => x.CustomOption).Cascade.SaveUpdate().LazyLoad();
        }
    }
}