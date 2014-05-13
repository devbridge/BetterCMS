using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    /// <summary>
    /// Request for html content update or creation.
    /// </summary>
    [Route("/contents/html/{ContentId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutHtmlContentRequest : RequestBase<SaveHtmlContentModel>, IReturn<PutHtmlContentResponse>
    {
        /// <summary>
        /// Gets or sets the content identifier.
        /// </summary>
        /// <value>
        /// The content identifier.
        /// </value>
        [DataMember]
        public Guid? ContentId { get; set; }
    }
}