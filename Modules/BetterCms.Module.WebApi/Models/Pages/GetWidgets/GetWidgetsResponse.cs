using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetWidgets
{
    [DataContract]
    public class GetWidgetsResponse : ListResponseBase<WidgetModel>
    {
    }
}