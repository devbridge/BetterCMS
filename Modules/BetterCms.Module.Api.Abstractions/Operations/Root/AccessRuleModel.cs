using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root
{
    [DataContract]
    [Serializable]
    public class AccessRuleModel
    {
        [DataMember]
        public string Identity { get; set; }

        [DataMember]
        public AccessLevel AccessLevel { get; set; }

        [DataMember]
        public bool IsForRole { get; set; }
    }
}
