using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost
{
    [DataContract]
    [Serializable]
    public class BlogPostModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the blog post content identifier.
        /// </summary>
        /// <value>
        /// The blog post content identifier.
        /// </value>
        [DataMember]
        public Guid ContentId { get; set; }

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
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the page layout id.
        /// </summary>
        /// <value>
        /// The page layout id.
        /// </value>
        [DataMember]
        public Guid? LayoutId { get; set; }

        /// <summary>
        /// Gets or sets the master page id.
        /// </summary>
        /// <value>
        /// The master page id.
        /// </value>
        [DataMember]
        public Guid? MasterPageId { get; set; }

        [DataMember]
        public List<CategoryModel> Categories { get; set; }

        /// <summary>
        /// Gets or sets the blog post author id.
        /// </summary>
        /// <value>
        /// The blog post author id.
        /// </value>
        [DataMember]
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        /// <value>
        /// The name of the author.
        /// </value>
        [DataMember]
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the blog posts main image id.
        /// </summary>
        /// <value>
        /// The blog post main image id.
        /// </value>
        [DataMember]
        public Guid? MainImageId { get; set; }

        /// <summary>
        /// Gets or sets the main image URL.
        /// </summary>
        /// <value>
        /// The main image URL.
        /// </value>
        [DataMember]
        public string MainImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the main image thumbnail URL.
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
        public Guid? FeaturedImageId { get; set; }

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
        public Guid? SecondaryImageId { get; set; }

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
        /// Gets or sets the blog post activation date.
        /// </summary>
        /// <value>
        /// The blog post activation date.
        /// </value>
        [DataMember]
        public DateTime ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the blog post expiration date.
        /// </summary>
        /// <value>
        /// The blog post expiration date.
        /// </value>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blog post is marked as archived.
        /// </summary>
        /// <value>
        /// <c>true</c> if blog post is marked as archived; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use canonical URL.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use canonical URL; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCanonicalUrl { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        [DataMember]
        public Guid? LanguageId { get; set; }

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
        public Guid? LanguageGroupIdentifier { get; set; }
    }
}