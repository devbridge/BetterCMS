using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser
{
    [DataContract]
    [Serializable]
    public class ValidUserModel
    {
        [DataMember]
        public bool Valid { get; set; }

        [DataMember]
        public Guid? UserId { get; set; }
    }
}
