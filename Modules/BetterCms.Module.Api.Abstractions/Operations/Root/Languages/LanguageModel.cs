using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages
{
    [DataContract]
    [Serializable]
    public class LanguageModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the language name.
        /// </summary>
        /// <value>
        /// The language name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public string Code { get; set; }        
    }
}