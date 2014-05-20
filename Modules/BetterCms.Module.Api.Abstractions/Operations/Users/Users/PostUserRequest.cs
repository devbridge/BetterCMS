using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Users.User;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    /// <summary>
    /// Request for user creation.
    /// </summary>
    [Route("/users/", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostUserRequest : RequestBase<SaveUserModel>, IReturn<PostUserResponse>
    {
    }
}
