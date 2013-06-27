using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetRenderedPageHtmlByPageUrl
{
    [DataContract]
    public class GetRenderedPageHtmlByPageUrlRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember(Order = 10, Name = "pageUrl")]
        public string PageUrl { get; set; }
    }
}