using System.Security.Principal;

namespace Devbridge.Platform.Core.Security
{
    public interface IPrincipalProvider
    {
        /// <summary>
        /// Gets the name of the get current principal.
        /// </summary>
        /// <value>
        /// The name of the get current principal.
        /// </value>
        string CurrentPrincipalName { get; }

        /// <summary>
        /// Gets the current principal.
        /// </summary>
        /// <returns>Current IPrincipal.</returns>
        IPrincipal GetCurrentPrincipal();
    }
}
