using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Languages
{
    [Route("/languages", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetLanguagesRequest : RequestBase<DataOptions>, IReturn<GetLanguagesResponse>
    {
    }
}