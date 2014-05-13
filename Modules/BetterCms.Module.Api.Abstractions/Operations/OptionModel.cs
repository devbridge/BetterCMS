using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    [Serializable]
    public class OptionModel
    {
        /// <summary>
        /// Gets or sets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the option default value.
        /// </summary>
        /// <value>
        /// The option default value.
        /// </value>
        [DataMember]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the option type.
        /// </summary>
        /// <value>
        /// The option type.
        /// </value>
        [DataMember]
        public OptionType Type { get; set; }

        /// <summary>
        /// Gets or sets the custom type identifier.
        /// </summary>
        /// <value>
        /// The custom type identifier.
        /// </value>
        [DataMember]
        public string CustomTypeIdentifier { get; set; }
    }
}
