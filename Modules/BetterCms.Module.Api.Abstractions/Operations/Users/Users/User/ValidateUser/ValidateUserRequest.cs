using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser
{
    [Route("/users/validate", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class ValidateUserRequest : RequestBase<ValidateUserModel>, IReturn<ValidateUserResponse>
    {
    }
}