using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    [Route("/roles/{RoleId}", Verbs = "GET")]
    [Route("/roles/by-name/{RoleName}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetRoleRequest : IReturn<GetRoleResponse>
    {
        [DataMember]
        public Guid? RoleId { get; set; }

        [DataMember]
        public string RoleName { get; set; }
    }
}