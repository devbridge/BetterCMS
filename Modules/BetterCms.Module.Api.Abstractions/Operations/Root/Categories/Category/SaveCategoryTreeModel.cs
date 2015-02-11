using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Data model for category save.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SaveCategoryTreeModel : SaveModelBase
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
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        [DataMember]
        public IList<SaveCategoryTreeNodeModel> Nodes { get; set; }

        [DataMember]
        public Guid CategoryTreeId { get; set; }

        [DataMember]
        public List<Guid> UseForCategorizableItems { get; set; }

        //        /// <summary>
//        /// Gets or sets the access rules.
//        /// </summary>
//        /// <value>
//        /// The access rules.
//        /// </value>
//        [DataMember]
//        public IList<AccessRuleModel> AccessRules { get; set; }
    }
}
