using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [Route("/page-properties/{PageId}", Verbs = "GET")]
    [Route("/page-properties/by-url/{PageUrl*}", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetPagePropertiesRequest : RequestBase<GetPagePropertiesModel>, IReturn<GetPagePropertiesResponse>
    {
        [DataMember]
        public System.Guid? PageId { get; set; }

        [DataMember]
        public string PageUrl { get; set; }
    }
}