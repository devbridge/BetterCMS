using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    [Route("/layouts", Verbs = "GET")]
    [DataContract]    
    public class GetLayoutsRequest : ListRequestBase, IReturn<GetLayoutsResponse>
    {
    }
}