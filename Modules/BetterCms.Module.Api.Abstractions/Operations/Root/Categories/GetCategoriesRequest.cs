using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    [Route("/categories", Verbs = "GET, POST")]
    [DataContract]
    [Serializable]
    public class GetCategoriesRequest : RequestBase<DataOptions>, IReturn<GetCategoriesResponse>
    {
    }
}