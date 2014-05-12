using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [Route("/contents/html/{ContentId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetHtmlContentRequest : IReturn<GetHtmlContentResponse>
    {
        [DataMember]
        public Guid ContentId { get; set; }
    }
}