using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;


namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    /// <summary>
    /// Request for html content update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostHtmlContentRequest : RequestBase<SaveHtmlContentModel>
    {
    }
}