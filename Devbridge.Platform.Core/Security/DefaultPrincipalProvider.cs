using System.Security.Principal;
using System.Threading;

namespace Devbridge.Platform.Core.Security
{
    public class DefaultPrincipalProvider : IPrincipalProvider
    {
        /// <summary>
        /// Gets the name of the get current principal.
        /// </summary>
        /// <value>
        /// The name of the get current principal.
        /// </value>
        public virtual string CurrentPrincipalName
        {
            get
            {
                var principal = GetCurrentPrincipal();

                if (principal != null && principal.Identity.IsAuthenticated)
                {
                    return principal.Identity.Name;
                }

                return "Anonymous";
            }
        }

        /// <summary>
        /// Gets the current principal.
        /// </summary>
        /// <returns>
        /// Current IPrincipal.
        /// </returns>
        public virtual IPrincipal GetCurrentPrincipal()
        {
            return Thread.CurrentPrincipal;
        }
    }
}
