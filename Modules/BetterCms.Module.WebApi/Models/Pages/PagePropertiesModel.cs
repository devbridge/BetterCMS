using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.WebApi.Models.Root;

namespace BetterCms.Module.WebApi.Models.Pages
{
    [DataContract]
    public class PagePropertiesModel : PageModel
    {
        [DataMember(Order = 500, Name = "description")]
        public string Description { get; set; }

        [DataMember(Order = 510, Name = "canonicalUrl")]
        public string CanonicalUrl { get; set; }

        [DataMember(Order = 520, Name = "customCss")]
        public string CustomCss { get; set; }

        [DataMember(Order = 530, Name = "customJs")]
        public string CustomJS { get; set; }

        [DataMember(Order = 540, Name = "useCanonicalUrl")]
        public bool UseCanonicalUrl { get; set; }

        [DataMember(Order = 550, Name = "useNoFollow")]
        public bool UseNoFollow { get; set; }

        [DataMember(Order = 560, Name = "useNoIndex")]
        public bool UseNoIndex { get; set; }

        [DataMember(Order = 570, Name = "imageId")]
        public Guid? ImageId { get; set; }

        [DataMember(Order = 580, Name = "imageUrl")]
        public Guid? ImageUrl { get; set; }

        [DataMember(Order = 590, Name = "secondaryImageId")]
        public Guid? SecondaryImageId { get; set; }

        [DataMember(Order = 600, Name = "secondaryImageUrl")]
        public Guid? SecondaryImageUrl { get; set; }

        [DataMember(Order = 610, Name = "featuredImageId")]
        public Guid? FeaturedImageId { get; set; }

        [DataMember(Order = 620, Name = "featuredImageUrl")]
        public Guid? FeaturedImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the page meta title.
        /// </summary>
        /// <value>
        /// The page meta title.
        /// </value>
        [DataMember(Order = 630, Name = "metaTitle")]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the page meta keywords.
        /// </summary>
        /// <value>
        /// The page meta keywords.
        /// </value>
        [DataMember(Order = 640, Name = "metaKeywords")]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the page meta description.
        /// </summary>
        /// <value>
        /// The page meta description.
        /// </value>
        [DataMember(Order = 650, Name = "metaDescription")]
        public string MetaDescription { get; set; }
    }
}