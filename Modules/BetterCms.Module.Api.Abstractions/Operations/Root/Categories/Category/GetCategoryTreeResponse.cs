using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Response with category data.
    /// </summary>
    [Serializable]
    [DataContract]
    public class GetCategoryTreeResponse : ResponseBase<CategoryTreeModel>
    {
//        /// <summary>
//        /// Gets or sets the access rules.
//        /// </summary>
//        /// <value>
//        /// The access rules.
//        /// </value>
//        [DataMember]
//        public IList<AccessRuleModel> AccessRules { get; set; }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        [DataMember]
        public IList<CategoryNodeModel> Nodes { get; set; }
    }
}
