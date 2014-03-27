using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root
{
    [DataContract]
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
