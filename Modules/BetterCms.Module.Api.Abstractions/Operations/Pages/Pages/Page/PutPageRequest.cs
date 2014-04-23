using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    [Route("/pages/{PageId}", Verbs = "PUT")]
    [DataContract]
    public class PutPageRequest : RequestBase<PageModel>, IReturn<PutPageResponse>
    {
        [DataMember]
        public System.Guid? PageId { get; set; }
    }
}