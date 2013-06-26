using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetRedirectById
{
    [DataContract]
    public class GetRedirectByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the redirect id.
        /// </summary>
        /// <value>
        /// The redirect id.
        /// </value>
        [DataMember(Order = 10, Name = "redirectId")]
        public System.Guid RedirectId { get; set; }
    }
}