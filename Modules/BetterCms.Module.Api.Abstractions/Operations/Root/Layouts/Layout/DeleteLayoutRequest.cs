using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    /// <summary>
    /// Request for layout delete operation.
    /// </summary>
    [Route("/layouts/{LayoutId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteLayoutRequest : DeleteRequestBase, IReturn<DeleteLayoutResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid LayoutId { get; set; }
    }
}