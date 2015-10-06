using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    [DataContract]
    [Serializable]
    public class GetUserRequest : RequestBase<GetUserModel>, IValidatableObject
    {
        [DataMember]
        public Guid? UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId == null && string.IsNullOrEmpty(UserName))
            {
                yield return new ValidationResult("A UserId or UserName should be provided.", new List<string> {"UserName"});
            }

            if (UserId != null && string.IsNullOrEmpty(UserName) || UserId == null && !string.IsNullOrEmpty(UserName)){}
            else
            {
                yield return new ValidationResult("A UserName field must be null if UserId is provided.", new List<string> {"UserId"});
            }
        }
    }
}