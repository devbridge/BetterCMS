using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    /// <summary>
    /// Role update response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutRoleResponse : SaveResponseBase
    {
    }
}
