using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Users.Users.User;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    /// <summary>
    /// Request for user creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostUserRequest : RequestBase<SaveUserModel>
    {
    }
}
