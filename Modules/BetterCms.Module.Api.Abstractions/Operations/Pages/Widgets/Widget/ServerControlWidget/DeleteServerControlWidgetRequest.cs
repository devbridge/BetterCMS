using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Server control widget delete request for REST.
    /// </summary>
    [Route("/widgets/server-control/{Id}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteServerControlWidgetRequest : DeleteRequestBase, IReturn<DeleteServerControlWidgetResponse>
    {
    }
}