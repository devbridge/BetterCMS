using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    [Route("/pages/{PageId}/contents/{PageContentId}", Verbs = "DELETE")]
    [DataContract]
    public class DeletePageContentRequest : IReturn<DeletePageContentResponse>
    {
        [DataMember]
        public Guid? PageContentId { get; set; }

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
