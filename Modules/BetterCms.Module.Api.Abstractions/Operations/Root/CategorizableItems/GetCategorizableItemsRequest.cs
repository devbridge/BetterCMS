using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    [Route("/categorizable-items", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetCategorizableItemsRequest : RequestBase<DataOptions>, IReturn<GetCategorizableItemsResponse>
    {
    }
}
