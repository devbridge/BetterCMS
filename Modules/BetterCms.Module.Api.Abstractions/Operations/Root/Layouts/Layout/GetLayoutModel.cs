using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [DataContract]
    [Serializable]
    public class GetLayoutModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether to include layout options.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include layout options; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeOptions { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to include layout regions.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include layout regions; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeRegions { get; set; }
    }
}
