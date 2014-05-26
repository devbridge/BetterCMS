using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [DataContract]
    [Serializable]
    public class GetPageContentsModel : DataOptions
    {
        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        /// <value>
        /// The region id.
        /// </value>
        [DataMember]
        public Guid? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember]
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished contents.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished contents; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeUnpublished { get; set; }
    }
}