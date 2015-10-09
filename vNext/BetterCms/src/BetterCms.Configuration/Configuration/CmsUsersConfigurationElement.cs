namespace BetterCms.Configuration
{
    /// <summary>
    /// Installation module configuration.
    /// </summary>
    public class CmsUsersConfigurationElement// : ConfigurationElement, ICmsUsersConfiguration
    {
        public bool CreateDefaultUserOnStart { get; set; } = true;

        public bool EnableCmsFormsAuthentication { get; set; } = true;
    }
}
