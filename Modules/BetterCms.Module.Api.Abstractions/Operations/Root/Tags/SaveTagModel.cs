using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags
{
    [DataContract]
    [Serializable]
    public class SaveTagModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        /// <value>
        /// The tag name.
        /// </value>
        [DataMember]
        public string Name { get; set; }        
    }
}