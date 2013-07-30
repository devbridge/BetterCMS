using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [Route("/contents/html/{ContentId}", Verbs = "GET")]
    [DataContract]
    public class GetHtmlContentRequest : IReturn<GetHtmlContentResponse>
    {
        [DataMember]
        public System.Guid ContentId { get; set; }
    }
}