using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Infrastructure
{
    [DataContract]
    [Serializable]
    public abstract class PutRequestBase<TData> : RequestBase<TData>
         where TData : new()
    {
        /// <summary>
        /// Gets or sets the updating entity identifier.
        /// </summary>
        /// <value>
        /// The updating entity identifier.
        /// </value>
        [DataMember]
        public Guid? Id { get; set; }
    }
}
