using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget.Options
{
    [DataContract]
    public class GetServerControlWidgetOptionsResponse : ListResponseBase<OptionModel>
    {
    }
}
