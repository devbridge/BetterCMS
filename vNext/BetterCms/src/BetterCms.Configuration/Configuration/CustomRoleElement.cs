namespace BetterCms.Configuration
{
    /// <summary>
    /// Configuration custom role element for security.
    /// </summary>
    public class CustomRoleElement
    {

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        /// <value>
        /// The permission.
        /// </value>
        public string Permission { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public string Roles { get; set; }
    }
}