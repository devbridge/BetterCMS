using System.Security.Principal;

using Devbridge.Platform.Core.Security;
using Devbridge.Platform.Core.Web.Web;

namespace Devbridge.Platform.Core.Web.Security
{
    public class DefaultWebPrincipalProvider : DefaultPrincipalProvider
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebPrincipalProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public DefaultWebPrincipalProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

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
