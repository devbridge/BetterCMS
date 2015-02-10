namespace Devbridge.Platform.Core.Configuration
{
    public interface IConfiguration
    {
        /// <summary>
        /// Gets the configuration of CMS database.
        /// </summary>
        IDatabaseConfiguration Database { get; }
    }
}
