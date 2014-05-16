using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    [Route("/pages/{PageId}/contents/{PageContentId}", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetPageContentRequest : RequestBase<GetPageContentModel>, IReturn<GetPageContentResponse>
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