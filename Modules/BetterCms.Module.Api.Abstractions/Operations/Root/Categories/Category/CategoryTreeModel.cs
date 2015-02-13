using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Category model.
    /// </summary>
    [Serializable]
    [DataContract]
    public class CategoryTreeModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Macro { get; set; }

        [DataMember]
        public IEnumerable<Guid> AvailableFor { get; set; }
    }
}