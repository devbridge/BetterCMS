using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetTagByName
{
    [DataContract]
    public class GetTagByNameRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        /// The name of the tag.
        /// </value>
        [DataMember(Order = 10, Name = "tagName")]
        public string TagName { get; set; }
    }
}