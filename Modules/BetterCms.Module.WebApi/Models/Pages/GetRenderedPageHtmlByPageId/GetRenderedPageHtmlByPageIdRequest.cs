using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetRenderedPageHtmlByPageId
{
    [DataContract]
    public class GetRenderedPageHtmlByPageIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        [DataMember(Order = 10, Name = "pageId")]
        public System.Guid PageId { get; set; }
    }
}