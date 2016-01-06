using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class ServerControlWidgetMap : EntitySubClassMapBase<ServerControlWidget>
    {
        public ServerControlWidgetMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("ServerControlWidgets");

            Map(x => x.Url).Not.Nullable();
        }
    }
}
