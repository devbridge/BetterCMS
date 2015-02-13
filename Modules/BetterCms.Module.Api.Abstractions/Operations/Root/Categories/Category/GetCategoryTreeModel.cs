using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Model for category getting.
    /// </summary>
    [Serializable]
    [DataContract]
    public class GetCategoryTreeModel
    {
//        /// <summary>
//        /// Gets or sets a value indicating whether to include access rules.
//        /// </summary>
//        /// <value>
//        ///   <c>true</c> if to include access rules; otherwise, <c>false</c>.
//        /// </value>
//        [DataMember]
//        public bool IncludeAccessRules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include nodes.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include nodes; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IncludeNodes { get; set; }
    }
}
