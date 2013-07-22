using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    [Route("/categories", Verbs = "GET")]
    public class GetCategoriesRequest : RequestBase<DataOptions>, IReturn<GetCategoriesResponse>
    {
    }
}