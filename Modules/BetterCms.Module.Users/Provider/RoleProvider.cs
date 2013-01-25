using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

using Autofac;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules.Registration;

using Common.Logging;

namespace BetterCms.Module.Users.Provider
{
    public class RoleProvider : System.Web.Security.RoleProvider
    {
        private static readonly ILog logger = LogManager.GetCurrentClassLogger();
        private readonly IModulesRegistration modulesRegistration;
        private readonly IUnitOfWork unitOfWork;

        private int cacheTimeoutInMinutes = 10;

        public RoleProvider()
        {
            var container = ContextScopeProvider.CreateChildContainer();
            modulesRegistration = container.Resolve<IModulesRegistration>();
            unitOfWork = container.Resolve<IUnitOfWork>();
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            try
            {
                var value = config["cacheTimeoutInMinutes"];
                if (value != null)
                {
                    cacheTimeoutInMinutes = Convert.ToInt32(value);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error during role cacheTimeoutInMinutes parameter reading. Default value will be used.", ex);
            }

            base.Initialize(name, config);
        }

        public override string ApplicationName { get; set; }

        public override string[] GetAllRoles()
        {
            return modulesRegistration.GetUserAccessRoles().Select(m => m.Name).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            if (HttpContext.Current.User == null)
            {
                return null;
            }

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            if (!Roles.CacheRolesInCookie)
            {
                var cacheKey = string.Format("UserRoles_{0}", username);
                if (HttpRuntime.Cache[cacheKey] != null)
                {
                    return (string[])HttpRuntime.Cache[cacheKey];
                }
                var userRoles = GetUserRoles(username);
                HttpRuntime.Cache.Insert(cacheKey, userRoles, null, DateTime.Now.AddMinutes(cacheTimeoutInMinutes), Cache.NoSlidingExpiration);
                return userRoles;
            }

            return GetUserRoles(username);
        }

        private string[] GetUserRoles(string username)
        {
            // TODO: implement.
            var userRoles = modulesRegistration.GetUserAccessRoles().Select(m => m.Name).ToList();
            userRoles.AddRange(new[] { "User", "Admin" });
            return userRoles.ToArray();
        }

        #region Not Implemented

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}