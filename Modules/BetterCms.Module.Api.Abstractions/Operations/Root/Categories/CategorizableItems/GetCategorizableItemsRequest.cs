using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.CategorizableItems
{
    [Route("/categories/categorizable-items", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetCategorizableItemsRequest : RequestBase<DataOptions>, IReturn<GetCategorizableItemsResponse>
    {
    }
}
