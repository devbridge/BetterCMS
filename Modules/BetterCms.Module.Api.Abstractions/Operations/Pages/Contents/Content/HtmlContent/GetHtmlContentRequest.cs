using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [DataContract]
    [Serializable]
    public class GetHtmlContentRequest : RequestBase<GetHtmlContentModel>
    {
        [DataMember]
        public Guid ContentId { get; set; }
    }
}