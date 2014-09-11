using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [DataContract]
    [Serializable]
    public class PageContentModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The content id.
        /// </value>
        [DataMember]
        public Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the parent page content identifier.
        /// </summary>
        /// <value>
        /// The parent page content identifier.
        /// </value>
        [DataMember]
        public Guid? ParentPageContentId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether content is published.
        /// </summary>
        /// <value>
        /// <c>true</c> if content is published; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        [DataMember]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        internal Type OriginalContentType { get; set; }

        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        /// <value>
        /// The content name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        [DataMember]
        public Guid RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember]
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the content order number in the region.
        /// </summary>
        /// <value>
        /// The content order number in the region.
        /// </value>
        [DataMember]
        public int Order { get; set; }
    }
}