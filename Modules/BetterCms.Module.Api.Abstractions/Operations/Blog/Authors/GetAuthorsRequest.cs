using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    [DataContract]
    [Route("/authors", Verbs = "GET")]
    [Serializable]
    public class GetAuthorsRequest : RequestBase<DataOptions>, IReturn<GetAuthorsResponse>
    {
    }
}