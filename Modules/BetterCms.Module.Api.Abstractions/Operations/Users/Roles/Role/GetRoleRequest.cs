using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    [Route("/roles/{RoleId}", Verbs = "GET")]
    [Route("/roles/by-name/{RoleName}", Verbs = "GET")]
    [DataContract]
    public class GetRoleRequest : IReturn<GetRoleResponse>
    {
        [DataMember]
        public System.Guid? RoleId { get; set; }

        [DataMember]
        public string RoleName { get; set; }
    }
}