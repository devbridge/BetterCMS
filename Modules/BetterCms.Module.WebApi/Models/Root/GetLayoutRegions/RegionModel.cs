using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetLayoutRegions
{
    [DataContract]
    public class RegionModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the region identifier.
        /// </summary>
        /// <value>
        /// The region identifier.
        /// </value>
        [DataMember(Order = 10, Name = "regionIdentifier")]
        public string RegionIdentifier { get; set; }
    }
}