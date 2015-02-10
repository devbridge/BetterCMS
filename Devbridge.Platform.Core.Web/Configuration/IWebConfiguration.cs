using Devbridge.Platform.Core.Configuration;

namespace Devbridge.Platform.Core.Web.Configuration
{
    public interface IWebConfiguration : IConfiguration
    {
        /// <summary>
        /// Gets or sets the web site URL.
        /// </summary>
        /// <value>
        /// The web site URL.
        /// </value>
        string WebSiteUrl { get; set; }
    }
}
