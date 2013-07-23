using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [Route("/contents/html/{ContentId}", Verbs = "GET")]
    [DataContract]
    public class GetHtmlContentRequest : RequestBase<GetHtmlContentModel>, IReturn<GetHtmlContentResponse>
    {
        [DataMember]
        public System.Guid ContentId
        {
            get
            {
                return Data.ContentId;
            }
            set
            {
                Data.ContentId = value;
            }
        }
    }
}