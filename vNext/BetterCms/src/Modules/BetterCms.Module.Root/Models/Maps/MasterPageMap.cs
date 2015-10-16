using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class MasterPageMap : EntityMapBase<MasterPage>
    {
        public MasterPageMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("MasterPages");

            References(f => f.Page).Column("PageId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Master).Column("MasterPageId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}
