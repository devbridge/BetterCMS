using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetRedirectById
{
    [DataContract]
    public class RedirectModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember(Order = 10, Name = "pageUrl")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>
        /// The redirect URL.
        /// </value>
        [DataMember(Order = 20, Name = "redirectUrl")]
        public string RedirectUrl { get; set; }
    }
}