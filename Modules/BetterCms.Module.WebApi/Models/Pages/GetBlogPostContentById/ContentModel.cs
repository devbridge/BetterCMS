using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetBlogPostContentById
{
    [DataContract]
    public class ContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content HTML.
        /// </summary>
        /// <value>
        /// The content HTML.
        /// </value>
        [DataMember(Order = 20, Name = "html")]
        public string Html { get; set; }
    }
}