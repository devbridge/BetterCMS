using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    /// <summary>
    /// Request for page content update or creation.
    /// </summary>
    [Route("/pages/{PageId}/contents/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutPageContentRequest : PutRequestBase<SavePageContentModel>, IReturn<PutPageContentResponse>
    {
        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        [DataMember]
        public Guid PageId { get; set; }
    }
}
