using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetHtmlContentById
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

        /// <summary>
        /// Gets or sets the content custom CSS.
        /// </summary>
        /// <value>
        /// The content custom CSS.
        /// </value>
        [DataMember(Order = 30, Name = "customCss")]
        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets the content custom java script.
        /// </summary>
        /// <value>
        /// The content custom java script.
        /// </value>
        [DataMember(Order = 40, Name = "customJavaScript")]
        public string CustomJavaScript { get; set; }
    }
}