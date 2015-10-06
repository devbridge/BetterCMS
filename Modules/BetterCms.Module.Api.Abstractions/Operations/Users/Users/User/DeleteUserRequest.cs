using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    /// <summary>
    /// User delete request for REST.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteUserRequest : DeleteRequestBase
    {
    }
}