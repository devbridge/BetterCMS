using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public abstract class SaveModelBase
    {
        [DataMember]
        public int Version { get; set; }
    }
}
