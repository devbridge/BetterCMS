using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget
{
    /// <summary>
    /// Request for server control widget creation.
    /// </summary>
    [Route("/widgets/server-control/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostServerControlWidgetRequest : RequestBase<SaveServerControlWidgetModel>, IReturn<PostServerControlWidgetResponse>
    {
    }
}
