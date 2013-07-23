using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    [Route("/categories", Verbs = "GET, POST")]
    [DataContract]
    public class GetCategoriesRequest : RequestBase<DataOptions>, IReturn<GetCategoriesResponse>
    {
    }
}