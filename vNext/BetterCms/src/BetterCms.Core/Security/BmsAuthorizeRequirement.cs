using BetterCms.Core.Services;
using Microsoft.AspNet.Authorization;

namespace BetterCms.Core.Security
{
    public class BmsAuthorizeRequirement: AuthorizationHandler<BmsAuthorizeRequirement>, IAuthorizationRequirement
    {
        private readonly ISecurityService securityService;
        private readonly string roles;

        public BmsAuthorizeRequirement(ISecurityService securityService, params string[] roles)
        {
            this.securityService = securityService;
            this.roles = string.Join(",", roles);
        }

        protected override void Handle(AuthorizationContext context, BmsAuthorizeRequirement requirement)
        {
            if (securityService.IsAuthorized(context.User, roles))
            {
                context.Succeed(requirement);
            }
        }
    }
}