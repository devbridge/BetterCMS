using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    /// <summary>
    /// Request base for delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class RequestDeleteModel
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DataMember]
        public int Version { get; set; }
    }
}