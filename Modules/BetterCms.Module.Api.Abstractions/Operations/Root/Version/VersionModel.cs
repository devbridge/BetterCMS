using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    [DataContract]
    [Serializable]
    public class VersionModel
    {
        [DataMember]
        public string Version { get; set; }
    }
}