using System.Web.Security;

using BetterCms.Module.Root.Services;
using BetterCms.Module.Users.Controllers;
using BetterCms.Module.Users.Provider;

using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Users.Services
{
    public class DefaultUserProfileUrlResolver : IUserProfileUrlResolver
    {
        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUserProfileUrlResolver" /> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public DefaultUserProfileUrlResolver(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the edit user profile URL.
        /// </summary>
        public string GetEditUserProfileUrl()
        {
            if (!Roles.Enabled || !(Roles.Provider is CmsRoleProvider) || !(Membership.Provider is CmsMembershipProvider))
            {
                return null;
            }

            return contextAccessor.ResolveActionUrl<UserProfileController>(f => f.EditProfile());
        }
    }
}