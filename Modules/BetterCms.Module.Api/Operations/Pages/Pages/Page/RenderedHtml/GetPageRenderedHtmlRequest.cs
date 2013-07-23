using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    [Route("/page-html/{PageId}", Verbs = "GET")]
    [Route("/page-html/by-url/{PageUrl*}", Verbs = "GET")]
    [DataContract]
    public class GetPageRenderedHtmlRequest : RequestBase<GetPageRenderedHtmlModel>, IReturn<GetPageRenderedHtmlResponse>
    {
        [DataMember]
        public System.Guid? PageId
        {
            get
            {
                return Data.PageId;
            }
            set
            {
                Data.PageId = value;
            }
        }

        [DataMember]
        public string PageUrl
        {
            get
            {
                return Data.PageUrl;
            }
            set
            {
                Data.PageUrl = value;
            }
        }
    }
}