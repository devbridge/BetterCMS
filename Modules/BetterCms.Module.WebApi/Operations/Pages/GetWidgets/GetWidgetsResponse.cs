using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetWidgets
{
    [DataContract]
    public class GetWidgetsResponse : ListResponseBase<WidgetModel>
    {
    }
}