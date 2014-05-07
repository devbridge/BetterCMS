using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    /// <summary>
    /// Page save request for REST.
    /// </summary>
    [Route("/pages/{PageId}", Verbs = "PUT")]
    [DataContract]
    public class PutPageRequest : RequestBase<SavePagePropertiesModel>, IReturn<PutPageResponse>
    {
        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        [DataMember]
        public Guid? PageId
        {
            get
            {
                return Data.Id;
            }
            set
            {
                Data.Id = value.HasValue ? value.Value : Guid.Empty;
            }
        }
    }
}