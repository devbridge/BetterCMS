using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [DataContract]
    [System.Serializable]
    public class PageTranslationModel
    {
        /// <summary>
        /// Gets or sets the translated page id.
        /// </summary>
        /// <value>
        /// The translated page id.
        /// </value>
        [DataMember]
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the translated page title.
        /// </summary>
        /// <value>
        /// The translated page title.
        /// </value>
        [DataMember]
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the translated page url.
        /// </summary>
        /// <value>
        /// The translated page url.
        /// </value>
        [DataMember]
        public string PageUrl { get; set; }
        
        /// <summary>
        /// Gets or sets the translated page language id.
        /// </summary>
        /// <value>
        /// The translated page language id.
        /// </value>
        [DataMember]
        public System.Guid? LanguageId { get; set; }
        
        /// <summary>
        /// Gets or sets the translated page language code.
        /// </summary>
        /// <value>
        /// The translated page language code.
        /// </value>
        [DataMember]
        public string LanguageCode { get; set; }
    }
}
