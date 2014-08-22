using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    [Route("/users", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetUsersRequest : RequestBase<GetUsersModel>, IReturn<GetUsersResponse>
    {
    }
}