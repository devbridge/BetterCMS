using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    [DataContract]
    [Serializable]
    public class GetRoleRequest : IValidatableObject
    {
        [DataMember]
        public Guid? RoleId { get; set; }

        [DataMember]
        public string RoleName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RoleId == null && string.IsNullOrEmpty(RoleName))
            {
                yield return new ValidationResult("A RoleId or RoleName should be provided.", new List<string> { "RoleName" });
            }

            if (RoleId != null && string.IsNullOrEmpty(RoleName) || RoleId == null && !string.IsNullOrEmpty(RoleName)) { }
            else
            {
                yield return new ValidationResult("A RoleName field must be null if RoleId is provided.", new List<string> { "RoleId" });
            }
        }
    }
}