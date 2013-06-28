using System;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [Route("/tags/{TagId}", Verbs = "GET")]
    [Route("/tags/name/{TagName}", Verbs = "GET")]
    public class GetTagRequest : RequestBase, IReturn<GetTagResponse>
    {
        public Guid? TagId { get; set; }

        public string TagName { get; set; }
    }
}