using System.Security.Principal;

using Devbridge.Platform.Core.Security;

namespace Devbridge.Platform.Core.Web.Security
{
    public class DefaultWebPrincipalProvider : DefaultPrincipalProvider
    {
        /// <summary>
        /// Gets the current principal.
        /// </summary>
        /// <returns>
        /// Current IPrincipal.
        /// </returns>
        public override IPrincipal GetCurrentPrincipal()
        {
            var currentHttpContext = httpContextAccessor.GetCurrent();

            if (currentHttpContext == null)
            {
                return base.GetCurrentPrincipal();
            }

            return currentHttpContext.User;
        }
    }
}
