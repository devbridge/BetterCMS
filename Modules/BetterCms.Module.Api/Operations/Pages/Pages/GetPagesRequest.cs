using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [Route("/pages", Verbs = "GET")]
    public class GetPagesRequest : RequestBase<GetPagesModel>, IReturn<GetPagesResponse>
    {
    }
}