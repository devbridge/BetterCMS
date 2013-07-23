using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [DataContract]
    public class GetWidgetsResponse : ListResponseBase<WidgetModel>
    {
    }
}