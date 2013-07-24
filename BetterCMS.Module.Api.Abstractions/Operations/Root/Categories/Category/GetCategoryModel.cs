using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [DataContract]
    public class GetCategoryModel
    {
        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        [DataMember]
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        [DataMember]
        public string CategoryName { get; set; }
    }
}