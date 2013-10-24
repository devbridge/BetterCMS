using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    [Route("/users", Verbs = "GET")]
    [DataContract]    
    public class GetUsersRequest : RequestBase<GetUsersModel>, IReturn<GetUsersResponse>
    {
    }
}