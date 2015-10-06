using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Roles
{
    [DataContract]
    [Serializable]
    public class GetRolesRequest : RequestBase<DataOptions>
    {
    }
}