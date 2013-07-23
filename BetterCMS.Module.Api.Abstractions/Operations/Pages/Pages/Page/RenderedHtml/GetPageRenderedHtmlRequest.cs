using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    [Route("/page-html/{PageId}", Verbs = "GET")]
    [Route("/page-html/by-url/{PageUrl*}", Verbs = "GET")]
    [DataContract]
    public class GetPageRenderedHtmlRequest : RequestBase<GetPageRenderedHtmlModel>, IReturn<GetPageRenderedHtmlResponse>
    {
    }
}