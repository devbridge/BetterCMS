using System;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [Route("/categories/{CategoryId}", Verbs = "GET")]
    [Route("/categories/by-name/{CategoryName}", Verbs = "GET")]
    public class GetCategoryRequest : RequestBase, IReturn<GetCategoryResponse>
    {
        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        public Guid? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; }
    }
}