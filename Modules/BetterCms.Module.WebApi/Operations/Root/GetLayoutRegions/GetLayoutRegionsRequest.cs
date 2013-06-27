using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.GetLayoutRegions
{
    [DataContract]
    public class GetLayoutRegionsRequest : ListRequestBase
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        /// <value>
        /// The layout id.
        /// </value>
        [DataMember(Order = 10, Name = "layoutId", IsRequired = true)]
        public System.Guid LayoutId { get; set; }
    }
}