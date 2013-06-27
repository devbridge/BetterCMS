using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.GetLayoutById
{
    [DataContract]
    public class GetLayoutByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the layout id.
        /// </summary>
        /// <value>
        /// The layout id.
        /// </value>
        [DataMember(Order = 10, Name = "layoutId")]
        public System.Guid LayoutId { get; set; }
    }
}