using System.Collections.Generic;
using System.Security.Principal;

namespace BetterCms.Module.Api.Infrastructure
{
    public class ApiPrincipal : IPrincipal
    {
        private readonly List<string> roles;

        public ApiPrincipal(ApiIdentity identity)
        {
            Identity = new GenericIdentity(identity.Name);

            if (identity.Roles != null)
            {
                roles = new List<string>(identity.Roles);
            }
            else
            {
                roles = new List<string>();
            }
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }
    }
}
