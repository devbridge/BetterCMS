using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.MediaManager.FindMedia
{
    [DataContract]
    public class FindMediaRequest : ListRequestBase
    {
        /// <summary>
        /// Gets or sets the search text.
        /// </summary>
        /// <value>
        /// The search text.
        /// </value>
        [DataMember(Order = 10, Name = "searchText")]
        public string SearchText { get; set; }
    }
}