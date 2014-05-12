using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options
{
    [Route("/pages/contents/{PageContentId}/options")]
    [DataContract]
    [System.Serializable]
    public class GetPageContentOptionsRequest : RequestBase<DataOptions>, IReturn<GetPageContentOptionsResponse>
    {
        [DataMember]
        public System.Guid PageContentId { get; set; }
    }
}
