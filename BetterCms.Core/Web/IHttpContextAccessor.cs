using System.Web;

namespace BetterCms.Core.Web
{
    /// <summary>
    /// Defines the contract to access current http context.
    /// </summary>
    public interface IHttpContextAccessor
    {
        /// <summary>
        /// Gets the current http context.
        /// </summary>
        /// <returns>Current http context instance.</returns>
        HttpContextBase GetCurrent();

        /// <summary>
        /// Returns the physical file path that corresponds to the specified virtual path on the Web server.
        /// </summary>
        /// <param name="path">The virtual path of the Web server.</param>
        /// <returns>The physical file path that corresponds to path.</returns>
        string MapPath(string path);
    }
}
