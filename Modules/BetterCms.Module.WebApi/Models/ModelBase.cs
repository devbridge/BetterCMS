using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models
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
        [DataMember(Order = 0, Name = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10000, Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [DataMember(Order = 10001, Name = "version")]
        public int Version { get; set; }
    }
}