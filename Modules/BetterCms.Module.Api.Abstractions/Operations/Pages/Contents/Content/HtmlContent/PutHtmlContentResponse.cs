using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    /// <summary>
    /// Response after content saving.
    /// </summary>
    [DataContract]
    public class PutHtmlContentResponse : ResponseBase<Guid?>
    {
    }
}