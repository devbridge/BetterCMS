using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Request for getting categories list.
    /// </summary>
    [Route("/categories", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetCategoriesRequest : RequestBase<GetCategoriesModel>, IReturn<GetCategoriesResponse>
    {
    }
}