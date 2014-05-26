using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Languages.Language
{
    [DataContract]
    [Serializable]
    public class SaveLanguageModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the media title.
        /// </summary>
        /// <value>
        /// The media title.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Code { get; set; }
    }
}