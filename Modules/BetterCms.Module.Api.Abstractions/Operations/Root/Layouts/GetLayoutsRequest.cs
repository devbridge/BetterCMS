using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Layouts
{
    [Route("/layouts", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetLayoutsRequest : RequestBase<DataOptions>, IReturn<GetLayoutsResponse>
    {
    }
}