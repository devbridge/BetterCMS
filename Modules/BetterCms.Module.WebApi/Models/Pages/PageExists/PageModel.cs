using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.PageExists
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
        [DataMember(Order = 10, Name = "exists")]
        public bool Exists { get; set; }

        /// <summary>
        /// Gets or sets the existing page id.
        /// </summary>
        /// <value>
        /// The existing page id.
        /// </value>
        [DataMember(Order = 20, Name = "pageId")]
        public System.Guid PageId { get; set; }
    }
}