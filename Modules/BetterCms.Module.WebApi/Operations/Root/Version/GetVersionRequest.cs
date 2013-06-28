using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    [DataContract]
    [Route("/version", Verbs = "GET")]
    public class GetVersionRequest : RequestBase, IReturn<GetVersionResponse>
    {
    }
}