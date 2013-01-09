using BetterCms.Core.Models;
using BetterCms.Module.Pages.Models.Maps.Predefined;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class ServerControlWidgetHistoryMap : ServerControlWidgetSubClassMapBase<ServerControlWidgetHistory>
    {
        public ServerControlWidgetHistoryMap()
            : base(PagesModuleDescriptor.ModuleName, "ServerControlWidgetHistory")
        {
        }
    }
}
