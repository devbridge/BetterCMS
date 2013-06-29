using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    public abstract class ModelBase
    {
        /// <summary>
        /// Gets or sets the model Id.
        /// </summary>
        /// <value>
        /// The model Id.
        /// </value>
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