using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPages
{
    [DataContract]
    public class GetPagesRequest : ListRequestBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include archived pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeArchived")]
        public bool IncludeArchived { get; set; }
    }
}