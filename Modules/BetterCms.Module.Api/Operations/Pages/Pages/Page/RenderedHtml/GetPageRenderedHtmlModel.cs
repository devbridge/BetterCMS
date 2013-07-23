using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    [DataContract]
    public class GetPageRenderedHtmlModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        [DataMember]
        public System.Guid? PageId { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember]
        public string PageUrl { get; set; }
    }
}