using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [Route("/tags/{TagId}", Verbs = "GET")]
    [Route("/tags/by-name/{TagName}", Verbs = "GET")]
    [DataContract]
    public class GetTagRequest : IReturn<GetTagResponse>
    {
        [DataMember]
        public Guid? TagId { get; set; }

        [DataMember]
        public string TagName { get; set; }
    }
}