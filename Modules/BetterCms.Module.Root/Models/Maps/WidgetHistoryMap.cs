using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class WidgetHistoryMap : EntitySubClassMapBase<WidgetHistory>
    {
        public WidgetHistoryMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("WidgetHistory");

            References(f => f.Category).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
