using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetTagById
{
    [DataContract]
    public class GetTagByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the tag id.
        /// </summary>
        /// <value>
        /// The tag id.
        /// </value>
        [DataMember(Order = 10, Name = "tagId")]
        public System.Guid TagId { get; set; }
    }
}