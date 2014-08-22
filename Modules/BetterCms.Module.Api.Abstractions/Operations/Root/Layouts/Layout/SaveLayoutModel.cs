using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    [DataContract]
    [Serializable]
    public class SaveLayoutModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout path.
        /// </summary>
        /// <value>
        /// The layout path.
        /// </value>
        [DataMember]
        public string LayoutPath { get; set; }

        /// <summary>
        /// Gets or sets the preview URL.
        /// </summary>
        /// <value>
        /// The preview URL.
        /// </value>
        [DataMember]
        public string PreviewUrl { get; set; }

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
        public System.Collections.Generic.IList<RegionSaveModel> Regions { get; set; }
    }
}
