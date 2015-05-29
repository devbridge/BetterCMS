using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    [DataContract]
    [Serializable]
    public class GetImagesModel : DataOptions, IFilterByTags, IFilterByCategories
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetImagesModel" /> class.
        /// </summary>
        public GetImagesModel()
        {
            IncludeFolders = true;
            IncludeImages = true;

            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        [DataMember]
        public System.Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include images.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include images; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeImages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived medias.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived medias; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include folders.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include folders; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeFolders { get; set; }
        
        [DataMember]
        public bool IncludeCategories { get; set; }

        /// <summary>
        /// Gets or sets the image tags for filtering.
        /// </summary>
        /// <value>
        /// The image tags for filtering.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember]
        public FilterConnector FilterByTagsConnector { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public List<Guid> FilterByCategories { get; set; }

        /// <summary>
        /// Gets or sets the categories names.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember]
        public List<string> FilterByCategoriesNames { get; set; }

        /// <summary>
        /// Gets or sets the categories filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember]
        public FilterConnector FilterByCategoriesConnector { get; set; }
    }
}