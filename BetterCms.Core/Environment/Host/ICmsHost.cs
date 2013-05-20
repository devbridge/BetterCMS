using System.Web;

namespace BetterCms.Core.Environment.Host
{
    /// <summary>
    /// Defines the contract for Better CMS application host.
    /// </summary>
    public interface ICmsHost
    {
        /// <summary>
        /// Called when the host application starts.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnApplicationStart(HttpApplication application);

        /// <summary>
        /// Called when the host application stops.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnApplicationEnd(HttpApplication application);

        /// <summary>
        /// Called when the host application throws unhandled error.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnApplicationError(HttpApplication application);

        /// <summary>
        /// Called when the host application begins a web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnBeginRequest(HttpApplication application);

        /// <summary>
        /// Called when the host application ends a web request.
        /// </summary>
        /// <param name="application">The host application.</param>
        void OnEndRequest(HttpApplication application);

        /// <summary>
        /// Called when the host application authenticates a web request.
        /// </summary>
        void OnAuthenticateRequest(HttpApplication application);

        /// <summary>
        /// Method to restarts the host application domain.
        /// </summary>
        void RestartApplicationHost();
    }
}
