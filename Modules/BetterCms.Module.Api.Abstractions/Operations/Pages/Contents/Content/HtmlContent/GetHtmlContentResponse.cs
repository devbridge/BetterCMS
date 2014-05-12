using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [DataContract]
    [Serializable]
    public class GetHtmlContentResponse : ResponseBase<HtmlContentModel>
    {
    }
}