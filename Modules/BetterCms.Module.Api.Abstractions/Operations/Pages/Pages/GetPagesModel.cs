using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Infrastructure.Enums;

namespace BetterCms.Module.Api.Operations.Pages.Pages
{
    [DataContract]
    [System.Serializable]
    public class GetPagesModel : DataOptions, IFilterByTags, IFilterByCategories
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagesModel" /> class.
        /// </summary>
        public GetPagesModel()
        {
            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished pages; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
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
        /// Gets or sets a value indicating whether to include page options collections to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  to include page options collections to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludePageOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include tags collections to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  to include tags collections to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeTags { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include metadata to results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include metadata to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeMetadata { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include master pages.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include master pages to results; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeMasterPages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include access rules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include access rules; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeAccessRules { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include Categories.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to includecategories; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeCategories { get; set; }

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