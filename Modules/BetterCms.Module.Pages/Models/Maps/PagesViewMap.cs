using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class PagesViewMap : EntityMapBase<PagesView>
    {
        public PagesViewMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("PagesView");

            Map(x => x.IsInSitemap).ReadOnly();
            References(x => x.Page).Column("Id").ReadOnly();
        }
    }
}
