using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/tags/{TagId}", Verbs = "DELETE")]
    [DataContract]
    public class DeleteTagRequest : RequestBase<RequestDeleteModel>, IReturn<DeleteTagResponse>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public Guid? TagId
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