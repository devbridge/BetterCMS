using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Root.GetCategoryByName
{
    [DataContract]
    public class GetCategoryByNameRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        [DataMember(Order = 10, Name = "categoryName")]
        public string CategoryName { get; set; }
    }
}