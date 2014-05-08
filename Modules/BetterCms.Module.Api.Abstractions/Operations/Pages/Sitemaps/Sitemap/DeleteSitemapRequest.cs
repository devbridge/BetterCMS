using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap
{
    /// <summary>
    /// Sitemap delete request.
    /// </summary>
    [Route("/sitemaps/{SitemapId}", Verbs = "DELETE")]
    [DataContract]
    public class DeleteSitemapRequest : RequestBase<RequestDeleteModel>, IReturn<DeleteSitemapResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid SitemapId
        {
            get
            {
                return this.Data.Id;
            }

            set
            {
                this.Data.Id = value;
            }
        }
    }
}