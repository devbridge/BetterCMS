using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Tags
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
        public string Name { get; set; }        
    }
}