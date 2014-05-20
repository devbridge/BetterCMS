using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    /// <summary>
    /// Response for html content delete operation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteHtmlContentResponse : DeleteResponseBase
    {
    }
}