using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    [Serializable]
    [DataContract]
    public class GetCategoryTreeModel
    {
        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        /// <value>
        /// The node id.
        /// </value>
        [DataMember]
        public Guid? NodeId { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>
        /// The language identifier.
        /// </value>
        [DataMember]
        public Guid? LanguageId { get; set; }
    }
}