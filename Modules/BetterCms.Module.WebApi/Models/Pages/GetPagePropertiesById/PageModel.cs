using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPagePropertiesById
{
    [DataContract]
    public class PageModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        [DataMember(Order = 10, Name = "pageUrl")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember(Order = 20, Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page description.
        /// </summary>
        /// <value>
        /// The page description.
        /// </value>
        [DataMember(Order = 30, Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        [DataMember(Order = 40, Name = "isPublished")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the page published date.
        /// </summary>
        /// <value>
        /// The page published date.
        /// </value>
        [DataMember(Order = 50, Name = "publishedOn")]
        public System.DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the page layout id.
        /// </summary>
        /// <value>
        /// The page layout id.
        /// </value>
        [DataMember(Order = 60, Name = "layoutId")]
        public System.Guid LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        [DataMember(Order = 70, Name = "categoryId")]
        public System.Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the featured main image id.
        /// </summary>
        /// <value>
        /// The main image id.
        /// </value>
        [DataMember(Order = 80, Name = "mainImageId")]
        public System.Guid? MainImageId { get; set; }

        /// <summary>
        /// Gets or sets the featured image id.
        /// </summary>
        /// <value>
        /// The featured image id.
        /// </value>
        [DataMember(Order = 90, Name = "featuredImageId")]
        public System.Guid? FeaturedImageId { get; set; }

        /// <summary>
        /// Gets or sets the secondary image id.
        /// </summary>
        /// <value>
        /// The secondary image id.
        /// </value>
        [DataMember(Order = 100, Name = "secondaryImageId")]
        public System.Guid? SecondaryImageId { get; set; }

        /// <summary>
        /// Gets or sets the canonical URL.
        /// </summary>
        /// <value>
        /// The canonical URL.
        /// </value>
        [DataMember(Order = 110, Name = "canonicalUrl")]
        public string CanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets the page custom CSS.
        /// </summary>
        /// <value>
        /// The page custom CSS.
        /// </value>
        [DataMember(Order = 120, Name = "customCss")]
        public string CustomCss { get; set; }

        /// <summary>
        /// Gets or sets the page custom JavaScript.
        /// </summary>
        /// <value>
        /// The page custom JavaScript.
        /// </value>
        [DataMember(Order = 130, Name = "customJs")]
        public string CustomJS { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use canonical URL; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 140, Name = "useCanonicalUrl")]
        public bool UseCanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use no follow.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use no follow; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 150, Name = "useNoFollow")]
        public bool UseNoFollow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use no index.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use no index; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 160, Name = "useNoIndex")]
        public bool UseNoIndex { get; set; }
    }
}