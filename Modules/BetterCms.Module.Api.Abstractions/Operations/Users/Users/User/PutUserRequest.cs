using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Users.Users.User
{
    /// <summary>
    /// Request for user update.
    /// </summary>
    [Route("/users/{UserId}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutUserRequest : RequestBase<SaveUserModel>, IReturn<PutUserResponse>
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember]
        public Guid? UserId { get; set; }
    }
}
