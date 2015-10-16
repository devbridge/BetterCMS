using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class WidgetMap : EntitySubClassMapBase<Widget>
    {
        public WidgetMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Widgets");

            HasMany(x => x.Categories).KeyColumn("WidgetId").Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0"); 
        }
    }
}
