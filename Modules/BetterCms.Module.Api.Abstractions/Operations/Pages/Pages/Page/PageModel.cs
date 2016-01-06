using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    [DataContract]
    [System.Serializable]
    public class PageModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember]
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        [DataMember]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the page published date.
        /// </summary>
        /// <value>
        /// The page published date.
        /// </value>
        [DataMember]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the page layout id.
        /// </summary>
        /// <value>
        /// The page layout id.
        /// </value>
        [DataMember]
        public System.Guid? LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the master page id.
        /// </summary>
        /// <value>
        /// The master page id.
        /// </value>
        [DataMember]
        public System.Guid? MasterPageId { get; set; }

        [DataMember]
        public IList<CategoryModel> Categories { get; set; }

        /// <summary>
        /// Gets or sets the page main image id.
        /// </summary>
        /// <value>
        /// The page main image id.
        /// </value>
        [DataMember]
        public System.Guid? MainImageId { get; set; }

        /// <summary>
        /// Gets or sets the main image URL.
        /// </summary>
        /// <value>
        /// The main image URL.
        /// </value>
        [DataMember]
        public string MainImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the main image thumbnauil URL.
        /// </summary>
        /// <value>
        /// The main image thumbnauil URL.
        /// </value>
        [Obsolete]
        [DataMember]
        public string MainImageThumbnauilUrl { get; set; }

        /// <summary>
        /// Gets or sets the main image thumbnail URL.
        /// </summary>
        /// <value>
        /// The main image thumbnail URL.
        /// </value>
        [DataMember]
        public string MainImageThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the main image caption.
        /// </summary>
        /// <value>
        /// The main image caption.
        /// </value>
        [DataMember]
        public string MainImageCaption { get; set; }

        /// <summary>
        /// Gets or sets the page featured image id.
        /// </summary>
        /// <value>
        /// The page featured image id.
        /// </value>
        [DataMember]
        public System.Guid? FeaturedImageId { get; set; }

        /// <summary>
        /// Gets or sets the featured image URL.
        /// </summary>
        /// <value>
        /// The featured image URL.
        /// </value>
        [DataMember]
        public string FeaturedImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the featured image thumbnail URL.
        /// </summary>
        /// <value>
        /// The featured image thumbnail URL.
        /// </value>
        [DataMember]
        public string FeaturedImageThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the featured image caption.
        /// </summary>
        /// <value>
        /// The featured image caption.
        /// </value>
        [DataMember]
        public string FeaturedImageCaption { get; set; }

        /// <summary>
        /// Gets or sets the page secondary image id.
        /// </summary>
        /// <value>
        /// The page secondary image id.
        /// </value>
        [DataMember]
        public System.Guid? SecondaryImageId { get; set; }

        /// <summary>
        /// Gets or sets the secondary image URL.
        /// </summary>
        /// <value>
        /// The secondary image URL.
        /// </value>
        [DataMember]
        public string SecondaryImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the secondary image thumbnail URL.
        /// </summary>
        /// <value>
        /// The secondary image thumbnail URL.
        /// </value>
        [DataMember]
        public string SecondaryImageThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the secondary image caption.
        /// </summary>
        /// <value>
        /// The secondary image caption.
        /// </value>
        [DataMember]
        public string SecondaryImageCaption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is marked as archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if page is marked as archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a page is a master page.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the page is a master page; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsMasterPage { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        [DataMember]
        public System.Guid? LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the language code.
        /// </summary>
        /// <value>
        /// The language code.
        /// </value>
        [DataMember]
        public string LanguageCode { get; set; }

        /// <summary>
        /// Gets or sets the language group identifier.
        /// </summary>
        /// <value>
        /// The language group identifier.
        /// </value>
        [DataMember]
        public System.Guid? LanguageGroupIdentifier { get; set; }
    }
}