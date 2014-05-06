using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    /// <summary>
    /// Request for page content update or creation.
    /// </summary>
    [Route("/pages/contents/{PageContentId}", Verbs = "PUT")]
    [DataContract]
    public class PutPageContentRequest : RequestBase<PageContentModel>, IReturn<PutPageContentResponse>
    {
        /// <summary>
        /// Gets or sets the page content identifier.
        /// </summary>
        /// <value>
        /// The page content identifier.
        /// </value>
        [DataMember]
        public Guid? PageContentId
        {
            get
            {
                return this.Data.Id;
            }

            set
            {
                this.Data.Id = value.HasValue ? value.Value : Guid.Empty;
            }
        }
    }
}
