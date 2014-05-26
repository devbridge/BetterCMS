using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    [Route("/pages/{PageId}", Verbs = "GET")]
    [Route("/pages/by-url/{PageUrl*}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetPageRequest : IReturn<GetPageResponse>
    {
        [DataMember]
        public Guid? PageId { get; set; }

        [DataMember]
        public string PageUrl { get; set; }
    }
}