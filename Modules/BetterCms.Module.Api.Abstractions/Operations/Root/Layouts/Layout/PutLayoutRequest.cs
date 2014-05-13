using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    /// <summary>
    /// Request for layout update or creation.
    /// </summary>
    [Route("/layouts/{LayoutId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutLayoutRequest : RequestBase<LayoutSaveModel>, IReturn<PutLayoutResponse>
    {
        /// <summary>
        /// Gets or sets the layout identifier.
        /// </summary>
        /// <value>
        /// The layout identifier.
        /// </value>
        [DataMember]
        public Guid? LayoutId { get; set; }
    }
}