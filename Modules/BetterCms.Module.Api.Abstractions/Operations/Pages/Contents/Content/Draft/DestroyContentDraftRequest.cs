using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.DestroyDraft
{
    /// <summary>
    /// Request for content's draft version destroy operation.
    /// </summary>
    [Route("/contents/{ContentId}/draft", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DestroyContentDraftRequest : DeleteRequestBase, IReturn<DestroyContentDraftResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid ContentId { get; set; }
    }
}