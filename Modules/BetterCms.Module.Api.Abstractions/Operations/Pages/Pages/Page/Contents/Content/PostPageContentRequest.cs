using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    /// <summary>
    /// Request for page content creation.
    /// </summary>
    [Route("/pages/{PageId}/contents", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostPageContentRequest : RequestBase<SavePageContentModel>, IReturn<PostPageContentResponse>
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
