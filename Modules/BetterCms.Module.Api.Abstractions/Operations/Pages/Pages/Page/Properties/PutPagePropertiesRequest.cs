using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    /// <summary>
    /// Page save request for REST.
    /// </summary>
    [Route("/page-properties/{PageId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutPagePropertiesRequest : RequestBase<SavePagePropertiesModel>, IReturn<PutPagePropertiesResponse>
    {
        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        [DataMember]
        public Guid? PageId { get; set; }
    }
}