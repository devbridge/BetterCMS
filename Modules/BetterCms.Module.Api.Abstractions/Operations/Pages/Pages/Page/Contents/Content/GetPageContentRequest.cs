using System;
using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    [Route("/pages/contents/{PageContentId}", Verbs = "GET")]
    [DataContract]
    public class GetPageContentRequest : IReturn<GetPageContentResponse>
    {
        /// <summary>
        /// Gets or sets the page content identifier.
        /// </summary>
        /// <value>
        /// The page content identifier.
        /// </value>
        [DataMember]
        public Guid? PageContentId { get; set; }
    }
}