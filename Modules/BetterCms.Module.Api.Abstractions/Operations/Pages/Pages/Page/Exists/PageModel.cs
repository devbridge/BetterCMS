using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists
{
    [DataContract]
    public class PageModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether page exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if page exists; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Exists { get; set; }

        /// <summary>
        /// Gets or sets the existing page id .
        /// </summary>
        /// <value>
        /// The existing page id.
        /// </value>
        [DataMember]
        public System.Guid? PageId { get; set; }
    }
}