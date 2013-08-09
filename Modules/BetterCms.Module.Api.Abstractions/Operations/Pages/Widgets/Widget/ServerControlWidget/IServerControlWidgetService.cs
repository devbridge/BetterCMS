using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    public interface IServerControlWidgetService
    {
        GetServerControlWidgetResponse Get(GetServerControlWidgetRequest request);

        IServerControlWidgetOptionsService Options { get; }
    }
}