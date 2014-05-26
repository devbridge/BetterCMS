using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions
{
    [DataContract]
    [Serializable]
    public class RegionSaveModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember]
        public string RegionIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }
    }
}