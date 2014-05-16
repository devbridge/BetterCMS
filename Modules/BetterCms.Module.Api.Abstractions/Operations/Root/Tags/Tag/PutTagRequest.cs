using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/tags/{TagId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutTagRequest : RequestBase<SaveTagModel>, IReturn<PutTagResponse>
    {
        /// <summary>
        /// Gets or sets the tag identifier.
        /// </summary>
        /// <value>
        /// The tag identifier.
        /// </value>
        [DataMember]
        public Guid? TagId { get; set; }
    }
}