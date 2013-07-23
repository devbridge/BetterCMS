using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    [Route("/widgets/server-control/{WidgetId}", Verbs = "GET")]
    [DataContract]
    public class GetServerControlWidgetRequest : RequestBase<GetServerControlWidgetModel>, IReturn<GetServerControlWidgetResponse>
    {
        [DataMember]
        public System.Guid WidgetId
        {
            get
            {
                return Data.WidgetId;
            }
            set
            {
                Data.WidgetId = value;
            }
        }
    }
}