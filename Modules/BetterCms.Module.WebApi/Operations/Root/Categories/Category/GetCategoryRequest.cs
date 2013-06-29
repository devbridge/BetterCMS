using System;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [Route("/categories/{CategoryId}", Verbs = "GET")]
    [Route("/categories/by-name/{CategoryName}", Verbs = "GET")]
    public class GetCategoryRequest : RequestBase, IReturn<GetCategoryResponse>
    {
        public Guid? CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}