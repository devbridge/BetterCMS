using BetterCms.Core.Models;
using BetterCms.Module.Pages.Models.Maps.Predefined;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class ServerControlWidgetMap : ServerControlWidgetSubClassMapBase<ServerControlWidget>
    {
        public ServerControlWidgetMap()
            : base(PagesModuleDescriptor.ModuleName, "ServerControlWidgets")
        {
        }
    }
}
