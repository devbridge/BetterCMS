using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [DataContract]
    public class GetLayoutRegionsModel : DataOptions
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        /// <value>
        /// The layout id.
        /// </value>
        [DataMember]
        public System.Guid LayoutId { get; set; }
    }
}