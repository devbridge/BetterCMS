using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Blog.GetBlogPostPropertiesById
{
    [DataContract]
    public class LayoutModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout path.
        /// </summary>
        /// <value>
        /// The layout path.
        /// </value>
        [DataMember(Order = 20, Name = "layoutPath")]
        public string LayoutPath { get; set; }

        /// <summary>
        /// Gets or sets the preview URL.
        /// </summary>
        /// <value>
        /// The preview URL.
        /// </value>
        [DataMember(Order = 30, Name = "previewUrl")]
        public string PreviewUrl { get; set; }     
    }
}