using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [Route("/pages", Verbs = "GET")]
    [DataContract]
    public class GetPagesRequest : RequestBase<GetPagesModel>, IReturn<GetPagesResponse>
    {
    }
}