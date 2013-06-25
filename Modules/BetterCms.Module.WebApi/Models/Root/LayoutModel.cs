using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root
{
    [DataContract]
    public class LayoutModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layout path.
        /// </summary>
        /// <value>
        /// The layout path.
        /// </value>
        [DataMember(Order = 20, Name = "layoutPath")]
        public string LayoutPath { get; set; }

        /// <summary>
        /// Gets or sets the preview URL.
        /// </summary>
        /// <value>
        /// The preview URL.
        /// </value>
        [DataMember(Order = 30, Name = "previewUrl")]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Gets or sets the module id.
        /// </summary>
        /// <value>
        /// The module id.
        /// </value>
        [DataMember(Order = 40, Name = "moduleId")]
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>
        /// The name of the module.
        /// </value>
        [DataMember(Order = 50, Name = "moduleName")]
        public string ModuleName { get; set; }        
    }
}