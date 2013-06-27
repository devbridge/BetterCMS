using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetPageById
{
    [DataContract]
    public class GetPageByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        [DataMember(Order = 10, Name = "pageId")]
        public System.Guid PageId { get; set; }
    }
}