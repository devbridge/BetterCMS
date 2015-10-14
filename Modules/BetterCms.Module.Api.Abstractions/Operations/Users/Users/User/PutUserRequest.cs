using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    /// <summary>
    /// Request for user update.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutUserRequest : PutRequestBase<SaveUserModel>
    {
    }
}
