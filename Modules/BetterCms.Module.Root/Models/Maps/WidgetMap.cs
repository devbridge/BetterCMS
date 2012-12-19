using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class WidgetMap : EntitySubClassMapBase<Widget>
    {
        public WidgetMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Widgets");

            References(f => f.Category).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
