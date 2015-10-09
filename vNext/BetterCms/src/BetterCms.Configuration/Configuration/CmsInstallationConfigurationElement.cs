namespace BetterCms.Configuration
{
    /// <summary>
    /// Installation module configuration.
    /// </summary>
    public class CmsInstallationConfigurationElement// : ConfigurationElement, ICmsInstallationConfiguration
    {
        public bool Install404ErrorPage { get; set; } = false;

        public bool Install500ErrorPage { get; set; } = false;

        public bool InstallDefaultPage { get; set; } = false;
    }
}
