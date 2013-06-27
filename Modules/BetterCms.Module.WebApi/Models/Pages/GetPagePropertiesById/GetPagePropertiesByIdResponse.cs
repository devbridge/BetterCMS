using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPagePropertiesById
{
    [DataContract]
    public class GetPagePropertiesByIdResponse : ResponseBase<PageModel>
    {
        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        [DataMember(Order = 10, Name = "layout")]
        public LayoutModel Layout { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember(Order = 20, Name = "category")]
        public CategoryModel Category { get; set; }

        /// <summary>
        /// Gets or sets the list of tags.
        /// </summary>
        /// <value>
        /// The list of tags.
        /// </value>
        [DataMember(Order = 30, Name = "tags")]
        public List<TagModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets the main image.
        /// </summary>
        /// <value>
        /// The main image.
        /// </value>
        [DataMember(Order = 40, Name = "mainImage")]
        public ImageModel MainImage { get; set; }

        /// <summary>
        /// Gets or sets the featured image.
        /// </summary>
        /// <value>
        /// The featured image.
        /// </value>
        [DataMember(Order = 50, Name = "featuredImage")]
        public ImageModel FeaturedImage { get; set; }

        /// <summary>
        /// Gets or sets the secondary image.
        /// </summary>
        /// <value>
        /// The secondary image.
        /// </value>
        [DataMember(Order = 60, Name = "secondaryImage")]
        public ImageModel SecondaryImage { get; set; }

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        [DataMember(Order = 70, Name = "metaData")]
        public MetadataModel MetaData { get; set; }

        /// <summary>
        /// Gets or sets the list of page contents.
        /// </summary>
        /// <value>
        /// The list of page contents.
        /// </value>
        [DataMember(Order = 80, Name = "pageContents")]
        public IList<ContentModel> PageContents { get; set; }   
    }
}