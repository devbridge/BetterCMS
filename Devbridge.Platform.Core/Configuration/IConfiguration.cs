namespace Devbridge.Platform.Core.Configuration
{
    public interface IConfiguration
    {
        /// <summary>
        /// Gets the configuration of database.
        /// </summary>
        IDatabaseConfiguration Database { get; }
    }
}
