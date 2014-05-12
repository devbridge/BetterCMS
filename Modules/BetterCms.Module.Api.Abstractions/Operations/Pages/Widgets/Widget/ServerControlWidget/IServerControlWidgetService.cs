using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    public interface IServerControlWidgetService
    {
        GetServerControlWidgetResponse Get(GetServerControlWidgetRequest request);

        PostServerControlWidgetResponse Post(PostServerControlWidgetRequest request);

        PutServerControlWidgetResponse Put(PutServerControlWidgetRequest request);

        DeleteServerControlWidgetResponse Delete(DeleteServerControlWidgetRequest request);

        IServerControlWidgetOptionsService Options { get; }
    }
}