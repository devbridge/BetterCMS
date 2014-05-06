using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Request for page properties update or creation.
    /// </summary>
    [Route("/page-properties/{PageId}", Verbs = "PUT")]
    [DataContract]
    public class PutPagePropertiesRequest : RequestBase<SavePagePropertiesModel>, IReturn<PutPagePropertiesResponse>
    {
        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The tag identifier.
        /// </value>
        [DataMember]
        public Guid? PageId
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