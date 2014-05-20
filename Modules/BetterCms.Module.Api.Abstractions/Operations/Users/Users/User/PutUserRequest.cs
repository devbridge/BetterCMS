using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    /// <summary>
    /// Request for user update.
    /// </summary>
    [Route("/users/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutUserRequest : PutRequestBase<SaveUserModel>, IReturn<PutUserResponse>
    {
    }
}
