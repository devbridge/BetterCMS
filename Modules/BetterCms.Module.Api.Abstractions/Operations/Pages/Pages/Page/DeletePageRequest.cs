using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    [Route("/pages/{PageId}", Verbs = "DELETE")]
    [DataContract]
    public class DeletePageRequest : IReturn<DeletePageResponse>
    {
        [DataMember]
        public System.Guid? PageId { get; set; }
    }
}