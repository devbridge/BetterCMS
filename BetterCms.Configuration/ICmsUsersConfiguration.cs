namespace BetterCms
{
    /// <summary>
    /// Users module configuration.
    /// </summary>
    public interface ICmsUsersConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether Better CMS should ask to create a default user on start.
        /// </summary>
        /// <value>
        /// <c>true</c> if Better CMS should ask to create a default user on start; otherwise, <c>false</c>.
        /// </value>
        bool CreateDefaultUserOnStart { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Better CMS forms authentication is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if Better CMS forms authentication is enabled; otherwise, <c>false</c>.
        /// </value>
        bool EnableCmsFormsAuthentication { get; set; }
    }
}
