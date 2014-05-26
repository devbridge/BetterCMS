using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    /// <summary>
    /// Role creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostRoleResponse : SaveResponseBase
    {
    }
}
