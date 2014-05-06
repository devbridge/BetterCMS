using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    public abstract class SaveModelBase
    {
        [DataMember]
        public int Version { get; set; }
    }
}
