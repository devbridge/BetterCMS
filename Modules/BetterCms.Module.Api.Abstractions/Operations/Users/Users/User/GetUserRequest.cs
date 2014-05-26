using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    [Route("/users/{UserId}", Verbs = "GET")]
    [Route("/users/by-username/{UserName}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetUserRequest : RequestBase<GetUserModel>, IReturn<GetUserResponse>
    {
        [DataMember]
        public Guid? UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }
    }
}