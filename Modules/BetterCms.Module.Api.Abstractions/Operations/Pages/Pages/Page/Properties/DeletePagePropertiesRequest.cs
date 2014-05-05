using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Request for page properties delete operation.
    /// </summary>
    [Route("/page-properties/{PageId}", Verbs = "DELETE")]
    [DataContract]
    public class DeletePagePropertiesRequest : RequestBase<RequestDeleteModel>, IReturn<DeletePagePropertiesResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
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