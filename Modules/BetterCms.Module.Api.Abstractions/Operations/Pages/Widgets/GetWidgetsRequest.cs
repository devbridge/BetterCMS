using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [Route("/widgets", Verbs = "GET")]
    [DataContract]
    public class GetWidgetsRequest : RequestBase<GetWidgetsModel>, IReturn<GetWidgetsResponse>
    {
    }
}