using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [DataContract]
    public class GetLayoutModel
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