using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PagesViewMap : EntityMapBase<PagesView>
    {
        public PagesViewMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("PagesView");

            Map(x => x.IsInSitemap).ReadOnly();
            References(x => x.Page).Column("Id").ReadOnly();
        }
    }
}
