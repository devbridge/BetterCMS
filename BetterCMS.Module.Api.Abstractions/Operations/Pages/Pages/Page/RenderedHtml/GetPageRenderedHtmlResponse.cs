using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.RenderedHtml
{
    [DataContract]
    public class GetPageRenderedHtmlResponse : ResponseBase<string>
    {
    }
}