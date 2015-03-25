using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    [DataContract]
    [Serializable]
    public class CategorizableItemModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the categorizable item name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }
    }
}
