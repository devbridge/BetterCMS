using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    /// <summary>
    /// Request for role update.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutRoleRequest : PutRequestBase<SaveRoleModel>
    {
    }
}
