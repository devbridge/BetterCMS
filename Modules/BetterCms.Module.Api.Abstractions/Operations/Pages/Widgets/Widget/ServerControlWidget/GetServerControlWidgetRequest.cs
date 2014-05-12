using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    [Route("/widgets/server-control/{WidgetId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetServerControlWidgetRequest : RequestBase<GetServerControlWidgetModel>, IReturn<GetServerControlWidgetResponse>
    {
        [DataMember]
        public Guid WidgetId { get; set; }
    }
}