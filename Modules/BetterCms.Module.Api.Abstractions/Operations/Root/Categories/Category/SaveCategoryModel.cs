using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [DataContract]
    [Serializable]
    public class SaveCategoryModel : SaveModelBase
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category tree id.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}