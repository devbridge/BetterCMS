using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public abstract class DeleteRequestBase : RequestBase<RequestDeleteModel>
    {
        /// <summary>
        /// Gets or sets the deleting entity identifier.
        /// </summary>
        /// <value>
        /// The deleting entity identifier.
        /// </value>
        [DataMember]
        public Guid Id { get; set; }
    }
}