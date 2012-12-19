using System;
using System.Web;

namespace BetterCms.Core.Environment.Host
{
    /// <summary>
    /// Defines the contract for BetterCMS application host.
    /// </summary>
    public interface ICmsHost
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        Version Version { get; }

        /// <summary>
        /// Called on host application start.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnApplicationStart(HttpApplication application);

        /// <summary>
        /// Called on host application end.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnApplicationEnd(HttpApplication application);

        /// <summary>
        /// Called on host application error.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnApplicationError(HttpApplication application);

        /// <summary>
        /// Called when host begins web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnBeginRequest(HttpApplication application);

        /// <summary>
        /// Called when host ends web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnEndRequest(HttpApplication application);

        /// <summary>
        /// Method to restarts the host application domain.
        /// </summary>
        void RestartApplicationHost();
    }
}
