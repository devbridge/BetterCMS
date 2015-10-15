using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties
{
    [DataContract]
    [System.Serializable]
    public class BlogPostPropertiesModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the blog post URL.
        /// </summary>
        /// <value>
        /// The blog post URL.
        /// </value>
        [DataMember]
        public string BlogPostUrl { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the intro text.
        /// </summary>
        /// <value>
        /// The intro text.
        /// </value>
        [DataMember]
        public string IntroText { get; set; }

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
        public IList<System.Guid> Categories { get; set; }

        /// <summary>
        /// Gets or sets the blog post author id.
        /// </summary>
        /// <value>
        /// The blog post author id.
        /// </value>
        [DataMember]
        public System.Guid? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the blog post activation date.
        /// </summary>
        /// <value>
        /// The blog post activation date.
        /// </value>
        [DataMember]
        public System.DateTime ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the blog post expiration date.
        /// </summary>
        /// <value>
        /// The blog post expiration date.
        /// </value>
        [DataMember]
        public System.DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blog post is marked as archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if blog post is marked as archived; otherwise, <c>false</c>.
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
    }
}