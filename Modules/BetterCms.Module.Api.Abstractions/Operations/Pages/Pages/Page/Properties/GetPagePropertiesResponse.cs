using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [DataContract]
    [System.Serializable]
    public class GetPagePropertiesResponse : ResponseBase<PagePropertiesModel>
    {
        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        [DataMember]
        public LayoutModel Layout { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public IList<CategoryModel> Categories { get; set; }

        /// <summary>
        /// Gets or sets the list of tags.
        /// </summary>
        /// <value>
        /// The list of tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<TagModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets the main image.
        /// </summary>
        /// <value>
        /// The main image.
        /// </value>
        [DataMember]
        public ImageModel MainImage { get; set; }

        /// <summary>
        /// Gets or sets the featured image.
        /// </summary>
        /// <value>
        /// The featured image.
        /// </value>
        [DataMember]
        public ImageModel FeaturedImage { get; set; }

        /// <summary>
        /// Gets or sets the secondary image.
        /// </summary>
        /// <value>
        /// The secondary image.
        /// </value>
        [DataMember]
        public ImageModel SecondaryImage { get; set; }

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        [DataMember]
        public MetadataModel MetaData { get; set; }
        
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [DataMember]
        public LanguageModel Language { get; set; }

        /// <summary>
        /// Gets or sets the list of page contents.
        /// </summary>
        /// <value>
        /// The list of page contents.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<PageContentModel> PageContents { get; set; }
        
        /// <summary>
        /// Gets or sets the list of page options.
        /// </summary>
        /// <value>
        /// The list of page options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<OptionValueModel> PageOptions { get; set; }
        
        /// <summary>
        /// Gets or sets the list of page translations.
        /// </summary>
        /// <value>
        /// The list of page translations.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<PageTranslationModel> PageTranslations { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<AccessRuleModel> AccessRules { get; set; }   
    }
}