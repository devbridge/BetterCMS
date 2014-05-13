using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [DataContract]
    [Serializable]
    public class GetLayoutResponse : ResponseBase<LayoutModel>
    {
        /// <summary>
        /// Gets or sets the list of layout options.
        /// </summary>
        /// <value>
        /// The list of layout options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<OptionModel> Options { get; set; }
        
        /// <summary>
        /// Gets or sets the list of layout regions.
        /// </summary>
        /// <value>
        /// The list of layout regions.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<RegionModel> Regions { get; set; }
    }
}