using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    /// <summary>
    /// User delete request for REST.
    /// </summary>
    [Route("/users/{UserId}", Verbs = "DELETE")]
    [DataContract]
    [Serializable]
    public class DeleteUserRequest : DeleteRequestBase, IReturn<DeleteUserResponse>
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember]
        public Guid UserId { get; set; }
    }
}