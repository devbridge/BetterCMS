using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.GetPagePropertiesById
{
    [DataContract]
    public class TagModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        /// <value>
        /// The tag name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }
    }
}