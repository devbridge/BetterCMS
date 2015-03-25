using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories
{
    /// <summary>
    /// Request for getting category trees list.
    /// </summary>
    [Route("/categorytrees", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetCategoryTreesRequest : RequestBase<GetCategoryTreesModel>, IReturn<GetCategoryTreesResponse>
    {
    }
}