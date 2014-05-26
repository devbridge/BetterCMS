using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    /// <summary>
    /// Request for html content update or creation.
    /// </summary>
    [Route("/contents/html/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutHtmlContentRequest : PutRequestBase<SaveHtmlContentModel>, IReturn<PutHtmlContentResponse>
    {
    }
}