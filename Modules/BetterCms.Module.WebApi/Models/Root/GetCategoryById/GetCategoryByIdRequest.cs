using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root.GetCategoryById
{
    [DataContract]
    public class GetCategoryByIdRequest : RequestBase
    {
        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        [DataMember(Order = 10, Name = "categoryId")]
        public System.Guid CategoryId { get; set; }
    }
}