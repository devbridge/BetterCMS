using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Redirects.Redirect
{
    [DataContract]
    [Serializable]
    public class SaveRedirectModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember]
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the redirect URL.
        /// </summary>
        /// <value>
        /// The redirect URL.
        /// </value>
        [DataMember]
        public string RedirectUrl { get; set; }
    }
}
