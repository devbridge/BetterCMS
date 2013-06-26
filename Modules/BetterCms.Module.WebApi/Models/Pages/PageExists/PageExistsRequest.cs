using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.PageExists
{
    [DataContract]
    public class PageExistsRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember(Order = 10, Name = "pageUrl")]
        public System.Guid PageUrl { get; set; }
    }
}