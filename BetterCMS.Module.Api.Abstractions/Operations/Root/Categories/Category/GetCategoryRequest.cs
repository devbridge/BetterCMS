using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    [Route("/categories/{CategoryId}", Verbs = "GET")]
    [Route("/categories/by-name/{CategoryName}", Verbs = "GET")]
    [DataContract]
    public class GetCategoryRequest : RequestBase<GetCategoryModel>, IReturn<GetCategoryResponse>
    {
        [DataMember]
        public System.Guid? CategoryId
        {
            get
            {
                return Data.CategoryId;
            }
            set
            {
                Data.CategoryId = value;
            }
        }

        [DataMember]
        public string CategoryName
        {
            get
            {
                return Data.CategoryName;
            }
            set
            {
                Data.CategoryName = value;
            }
        }
    }
}