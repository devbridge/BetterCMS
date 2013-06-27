using System;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tag
{
    [Route("/tag/id/{TagId}", Verbs = "GET")]
    [Route("/tag/{TagName*}", Verbs = "GET")]
    public class GetTagRequest : RequestBase, IReturn<GetTagResponse>
    {
        public Guid? TagId { get; set; }

        public string TagName { get; set; }
    }
}