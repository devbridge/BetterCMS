using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root
{
    [DataContract]
    public class ModuleModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the module name.
        /// </summary>
        /// <value>
        /// The module name.
        /// </value>
        [DataMember(Order = 10, Name = "name")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the module description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        [DataMember(Order = 20, Name = "description")]
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the module version.
        /// </summary>
        /// <value>
        /// The module version.
        /// </value>
        [DataMember(Order = 30, Name = "moduleVersion")]
        public virtual string ModuleVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ModuleModel" /> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 40, Name = "enabled")]
        public virtual bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the module order number.
        /// </summary>
        /// <value>
        /// The module order number.
        /// </value>
        [DataMember(Order = 50, Name = "order")]
        public virtual int Order { get; set; }
    }
}