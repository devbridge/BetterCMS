using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    [Route("/roles", Verbs = "GET")]
    [DataContract]
    public class GetRolesRequest : RequestBase<DataOptions>, IReturn<GetRolesResponse>
    {
    }
}