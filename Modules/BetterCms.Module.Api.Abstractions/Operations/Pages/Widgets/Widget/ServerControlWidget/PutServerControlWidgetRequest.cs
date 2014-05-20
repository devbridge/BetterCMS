using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Request for server control widget update.
    /// </summary>
    [Route("/widgets/server-control/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutServerControlWidgetRequest : PutRequestBase<SaveServerControlWidgetModel>, IReturn<PutServerControlWidgetResponse>
    {
    }
}
