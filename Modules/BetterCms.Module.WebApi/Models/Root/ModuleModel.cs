using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root
{
    [DataContract]
    public class ModuleModel : ModelBase
    {
        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual string Description { get; set; }

        [DataMember]
        public virtual string ModuleVersion { get; set; }

        [DataMember]
        public virtual bool Enabled { get; set; }
    }
}