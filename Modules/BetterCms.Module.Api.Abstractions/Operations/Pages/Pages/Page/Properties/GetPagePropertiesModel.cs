using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [DataContract]
    [System.Serializable]
    public class GetPagePropertiesModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include tags.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include tags; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include category.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include category; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeCategories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include layout.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include layout; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeLayout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include image; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include meta data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include meta data; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeMetaData { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include language.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include language; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include page contents.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include page contents; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludePageContents { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include page options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include page options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludePageOptions { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include page translations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include page translations; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludePageTranslations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include access rules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include access rules; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeAccessRules { get; set; }
    }
}