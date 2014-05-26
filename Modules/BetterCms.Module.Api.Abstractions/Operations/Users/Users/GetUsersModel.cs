using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Users.Users
{
    [DataContract]
    [Serializable]
    public class GetUsersModel : Infrastructure.DataOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUsersModel" /> class.
        /// </summary>
        public GetUsersModel()
        {
            FilterByRolesConnector = Infrastructure.Enums.FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<string> FilterByRoles { get; set; }

        /// <summary>
        /// Gets or sets the roles filter connector.
        /// </summary>
        /// <value>
        /// The roles filter connector.
        /// </value>
        [DataMember]
        public Infrastructure.Enums.FilterConnector FilterByRolesConnector { get; set; }
    }
}
