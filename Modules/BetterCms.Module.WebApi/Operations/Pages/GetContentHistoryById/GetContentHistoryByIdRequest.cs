using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetContentHistoryById
{
    [DataContract]
    public class GetPageContentHistoryByPageContentIdRequest : ListRequestBase
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        [DataMember(Order = 10, Name = "contentId")]
        public System.Guid ContentId { get; set; }
    }
}