using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Widgets
{
    [Route("/widgets", Verbs = "GET")]
    public class GetWidgetsRequest : RequestBase<GetWidgetsModel>, IReturn<GetWidgetsResponse>
    {
    }
}