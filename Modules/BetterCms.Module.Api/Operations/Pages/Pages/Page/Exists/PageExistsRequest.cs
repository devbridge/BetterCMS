using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    [Route("/page-exists/{PageUrl*}")]
    public class PageExistsRequest : RequestBase
    {
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