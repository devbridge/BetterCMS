using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Version
{
    [DataContract]
    [Route("/current-version", Verbs = "GET")]
    [Serializable]
    public class GetVersionRequest : RequestBase<GetVersionModel>, IReturn<GetVersionResponse>
    {
    }
}