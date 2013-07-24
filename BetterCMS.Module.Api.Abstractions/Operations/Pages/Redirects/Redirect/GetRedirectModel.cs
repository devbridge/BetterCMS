using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [DataContract]
    public class GetRedirectModel
    {
        /// <summary>
        /// Gets or sets the redirect id.
        /// </summary>
        /// <value>
        /// The redirect id.
        /// </value>
        [DataMember]
        public System.Guid RedirectId { get; set; }
    }
}