using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;

using Autofac;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Users.Services;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Dependencies;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Provider
{
    public class CmsRoleProvider : System.Web.Security.RoleProvider
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The role service
        /// </summary>
        private readonly IRoleService roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsRoleProvider" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="roleService">The role service.</param>
        internal CmsRoleProvider(IRepository repository, IUnitOfWork unitOfWork = null, IRoleService roleService = null)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.roleService = roleService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsRoleProvider" /> class.
        /// </summary>
// ReSharper disable UnusedMember.Global
        public CmsRoleProvider()
// ReSharper restore UnusedMember.Global
        {
        }

        /// <summary>
        /// Gets or sets the name of the application to store and retrieve role information for.
        /// </summary>
        /// <returns>The name of the application to store and retrieve role information for.</returns>
        public override string ApplicationName { get; set; }

        /// <summary>
        /// Gets a list of all the roles for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data source for the configured applicationName.
        /// </returns>
        public override string[] GetAllRoles()
        {
            if (repository == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {                    
                    return GetAllRoles(container.Resolve<IRepository>());
                }               
            }

            return GetAllRoles(repository);
        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured applicationName.
        /// </summary>
        /// <param name="username">The user to return a list of roles for.</param>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            if (repository == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return GetRolesForUser(container.Resolve<IRepository>(), username);
                }
            }

            return GetRolesForUser(repository, username);
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            if (repository == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return IsUserInRole(container.Resolve<IRepository>(), username, roleName);
                }
            }

            return IsUserInRole(repository, username, roleName);
        }

        /// <summary>
        /// Adds a new role to the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            if (roleService == null || unitOfWork == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    CreateRole(container.Resolve<IRoleService>(), container.Resolve<IUnitOfWork>(), roleName);
                }
            }
            else
            {
                CreateRole(roleService, unitOfWork, roleName);
            }
        }

        /// <summary>
        /// Removes a role from the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if <paramref name="roleName" /> has one or more members and do not delete <paramref name="roleName" />.</param>
        /// <returns>
        /// true if the role was successfully deleted; otherwise, false.
        /// </returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (roleService == null || unitOfWork == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return DeleteRole(container.Resolve<IRoleService>(), container.Resolve<IUnitOfWork>(), roleName, throwOnPopulatedRole);
                }
            }

            return DeleteRole(roleService, unitOfWork, roleName, throwOnPopulatedRole);
        }

        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to search for in the data source.</param>
        /// <returns>
        /// true if the role name already exists in the data source for the configured applicationName; otherwise, false.
        /// </returns>
        public override bool RoleExists(string roleName)
        {
            if (repository == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return RoleExists(container.Resolve<IRepository>(), roleName);
                }
            }

            return RoleExists(repository, roleName);
        }

        /// <summary>
        /// Adds the users to roles.
        /// </summary>
        /// <param name="userNames">The user names.</param>
        /// <param name="roleNames">The role names.</param>
        public override void AddUsersToRoles(string[] userNames, string[] roleNames)
        {
            if (repository == null || unitOfWork == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    AddUsersToRoles(container.Resolve<IRepository>(), container.Resolve<IUnitOfWork>(), userNames, roleNames);
                }
            }
            else
            {

                AddUsersToRoles(repository, unitOfWork, userNames, roleNames);
            }
        }

        /// <summary>
        /// Removes the users from roles.
        /// </summary>
        /// <param name="userNames">The user names.</param>
        /// <param name="roleNames">The role names.</param>
        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            if (repository == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    RemoveUsersFromRoles(container.Resolve<IRepository>(), container.Resolve<IUnitOfWork>(), userNames, roleNames);
                }
            }
            else
            {
                RemoveUsersFromRoles(repository, unitOfWork, userNames, roleNames);
            }
        }

        /// <summary>
        /// Gets a list of users in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to get the list of users for.</param>
        /// <returns>
        /// A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        /// </returns>
        public override string[] GetUsersInRole(string roleName)
        {
            if (repository == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return GetUsersInRole(container.Resolve<IRepository>(), roleName);
                }
            }

            return GetUsersInRole(repository, roleName);
        }

        /// <summary>
        /// Finds the users in role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="userNameToMatch">The user name to match.</param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string userNameToMatch)
        {
            if (repository == null)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    return FindUsersInRole(container.Resolve<IRepository>(), roleName, userNameToMatch);
                }
            }

            return FindUsersInRole(repository, roleName, userNameToMatch);
        }

        private static void CreateRole(IRoleService roleService, IUnitOfWork unitOfWork, string roleName)
        {
            unitOfWork.BeginTransaction();
            roleService.CreateRole(roleName);
            unitOfWork.Commit();
        }

        private static string[] GetAllRoles(IRepository repository)
        {
            return repository.AsQueryable<Models.Role>().Select(r => r.Name).ToArray();
        }

        private static string[] GetRolesForUser(IRepository repository, string username)
        {
            return repository
                .AsQueryable<Models.UserRole>(userRole => userRole.User.UserName == username)
                .Select(userRole => userRole.Role.Name)
                .Distinct()
                .ToArray();
        }

        private static bool IsUserInRole(IRepository repository, string username, string roleName)
        {
            return repository
                .AsQueryable<Models.User>(u => u.UserName == username
                    && u.UserRoles.Any(ur => ur.Role.Name == roleName))
                .Select(u => u.UserName)
                .FirstOrDefault() != null;
        }

        private static bool DeleteRole(IRoleService roleService, IUnitOfWork unitOfWork, string roleName, bool throwOnPopulatedRole)
        {
            unitOfWork.BeginTransaction();
            var role = roleService.DeleteRole(roleName, throwOnPopulatedRole);
            unitOfWork.Commit();

            Events.UserEvents.Instance.OnRoleDeleted(role);

            return true;
        }

        private static bool RoleExists(IRepository repository, string roleName)
        {
            return repository
                .AsQueryable<Models.Role>(role => role.Name == roleName)
                .Select(u => u.Name)
                .FirstOrDefault() != null;
        }

        private static void AddUsersToRoles(IRepository repository, IUnitOfWork unitOfWork, string[] userNames, string[] roleNames)
        {
            unitOfWork.BeginTransaction();

            // Get roles future query
            var distinctRoleNames = roleNames.Distinct().ToArray();
            var roleIdsQuery = repository
                .AsQueryable<Models.Role>(role => distinctRoleNames.Contains(role.Name))
                .Select(role => new
                                    {
                                        Id = role.Id,
                                        Name = role.Name
                                    })
                .ToFuture();

            // Get users future query
            var distinctUserNames = userNames.Distinct().ToArray();
            var userIdsQuery = repository
                .AsQueryable<Models.User>(user => distinctUserNames.Contains(user.UserName))
                .Select(user => new
                                    {
                                        Id = user.Id,
                                        Name = user.UserName
                                    })
                .ToFuture();

            // Get already assigned roles
            var alreadyAssigned = repository
                .AsQueryable<Models.UserRole>(userRole => distinctUserNames.Contains(userRole.User.UserName))
                .Select(userRole => new
                                        {
                                            UserId = userRole.User.Id,
                                            RoleId = userRole.Role.Id
                                        })
                .ToFuture()
                .ToList();

            // Validate roles
            var roles = roleIdsQuery.ToList();
            roleNames
                .Where(roleName => roles.All(role => roleName != role.Name))
                .ForEach(roleName =>
                    { throw new ProviderException(string.Format("Role {0} does not exist.", roleName)); });

            // Validate users
            var users = userIdsQuery.ToList();
            userNames
                .Where(userName => users.All(user => userName != user.Name))
                .ForEach(userName =>
                    { throw new ProviderException(string.Format("User {0} does not exist.", userName)); });

            // Add users to roles
            roles
                .ForEach(role => users
                    .ForEach(user =>
                        {
                            if (!alreadyAssigned.Any(a => a.UserId == user.Id && a.RoleId == role.Id))
                            {
                                var userRole = new Models.UserRole
                                                   {
                                                       User = repository.AsProxy<Models.User>(user.Id),
                                                       Role = repository.AsProxy<Models.Role>(role.Id), 
                                                   };
                                repository.Save(userRole);
                            }
                        }));

            unitOfWork.Commit();
        }

        private static void RemoveUsersFromRoles(IRepository repository, IUnitOfWork unitOfWork, string[] userNames, string[] roleNames)
        {
            unitOfWork.BeginTransaction();

            var distinctRoleNames = roleNames.Distinct().ToList();
            var futureQueries = new List<IEnumerable<Models.Role>>();

            foreach (var userName in userNames.Distinct())
            {
                var futureQuery = repository
                    .AsQueryable<Models.UserRole>(userRole => userRole.User.UserName == userName
                        && distinctRoleNames.Contains(userRole.Role.Name))
                    .Select(userRole => new Models.Role
                        {
                            Id = userRole.Id,
                            Version = userRole.Version
                        })
                    .ToFuture();
                
                futureQueries.Add(futureQuery);
            }

            futureQueries.ForEach(futureQuery => 
                futureQuery.ToList().ForEach(role => 
                    repository.Delete<Models.UserRole>(role.Id, role.Version)));

            unitOfWork.Commit();
        }

        private static string[] GetUsersInRole(IRepository repository, string roleName)
        {
            return repository
                .AsQueryable<Models.UserRole>(userRole => userRole.Role.Name == roleName)
                .Select(userRole => userRole.User.UserName)
                .Distinct()
                .ToArray();
        }

        private static string[] FindUsersInRole(IRepository repository, string roleName, string userNameToMatch)
        {
            return repository
                .AsQueryable<Models.UserRole>(userRole => userRole.Role.Name == roleName
                    && userRole.User.UserName.Contains(userNameToMatch))
                .Select(userRole => userRole.User.UserName)
                .Distinct()
                .ToArray();
        }
    }
}