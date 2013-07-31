using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [Route("/categories/{CategoryId}", Verbs = "GET")]
    [Route("/categories/by-name/{CategoryName}", Verbs = "GET")]
    [DataContract]
    public class GetCategoryRequest : IReturn<GetCategoryResponse>
    {
        [DataMember]
        public System.Guid? CategoryId { get; set; }

        [DataMember]
        public string CategoryName { get; set; }
    }
}