using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    /// <summary>
    /// Page content delete request for REST.
    /// </summary>
    [Route("/pages/{PageId}/contents/{PageContentId}", Verbs = "DELETE")]
    [DataContract]
    public class DeletePageContentRequest : DeleteRequestBase,  IReturn<DeletePageContentResponse>
    {
        /// <summary>
        /// Gets or sets the page content identifier.
        /// </summary>
        /// <value>
        /// The page content identifier.
        /// </value>
        [DataMember]
        public Guid PageContentId { get; set; }

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
