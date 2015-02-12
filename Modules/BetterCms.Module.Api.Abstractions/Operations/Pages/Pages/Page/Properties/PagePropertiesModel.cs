using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [DataContract]
    [System.Serializable]
    public class PagePropertiesModel : ModelBase
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
        /// Gets or sets the page description.
        /// </summary>
        /// <value>
        /// The page description.
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

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        [DataMember]
        public List<System.Guid> Categories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether page is marked as archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if page is marked as archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets the featured main image id.
        /// </summary>
        /// <value>
        /// The main image id.
        /// </value>
        [DataMember]
        public System.Guid? MainImageId { get; set; }

        /// <summary>
        /// Gets or sets the featured image id.
        /// </summary>
        /// <value>
        /// The featured image id.
        /// </value>
        [DataMember]
        public System.Guid? FeaturedImageId { get; set; }

        /// <summary>
        /// Gets or sets the secondary image id.
        /// </summary>
        /// <value>
        /// The secondary image id.
        /// </value>
        [DataMember]
        public System.Guid? SecondaryImageId { get; set; }

        /// <summary>
        /// Gets or sets the page custom CSS.
        /// </summary>
        /// <value>
        /// The page custom CSS.
        /// </value>
        [DataMember]
        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets the page custom JavaScript.
        /// </summary>
        /// <value>
        /// The page custom JavaScript.
        /// </value>
        [DataMember]
        public string CustomJavaScript { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use canonical URL; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use no follow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use no follow; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseNoFollow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use no index.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use no index; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseNoIndex { get; set; }

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
        /// Gets or sets the language group identifier.
        /// </summary>
        /// <value>
        /// The language group identifier.
        /// </value>
        [DataMember]
        public System.Guid? LanguageGroupIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the type of the access (http vs https).
        /// </summary>
        /// <value>
        /// The type of the access (http vs https).
        /// </value>
        [DataMember]
        public virtual ForceProtocolType ForceAccessProtocol { get; set; }
    }
}