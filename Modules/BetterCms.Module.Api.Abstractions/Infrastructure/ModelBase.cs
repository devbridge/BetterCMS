using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public abstract class ModelBase
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }

        [DataMember]
        public DateTime LastModifiedOn { get; set; }

        [DataMember]
        public string LastModifiedBy { get; set; }

        [DataMember]
        public int Version { get; set; }
    }
}