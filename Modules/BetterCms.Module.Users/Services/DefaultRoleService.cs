using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Models;

namespace BetterCms.Module.Users.Services
{
    /// <summary>
    /// Service implementation for managing user roles
    /// </summary>
    public class DefaultRoleService : IRoleService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRoleService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultRoleService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// Created role entity
        /// </returns>
        public Role CreateRole(string name, string description = null)
        {
            return SaveRole(Guid.Empty, 1, name, description);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// Updated role entity
        /// </returns>
        public Role UpdateRole(Guid id, int version, string name, string description = null)
        {
            return SaveRole(id, version, name, description);
        }

        /// <summary>
        /// Deletes the role by specified role name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if role has one or more members and do not delete role.</param>
        /// <returns>
        /// Deleted role entity
        /// </returns>
        public Role DeleteRole(string name, bool throwOnPopulatedRole)
        {
            var role = repository
                .AsQueryable<Role>(r => name == r.Name)
                .FetchMany(r => r.UserRoles)
                .ToList()
                .FirstOne();

            DeleteRole(role, throwOnPopulatedRole);

            return role;
        }

        /// <summary>
        /// Deletes the role by specified role id and version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if role has one or more members and do not delete role.</param>
        /// <returns>
        /// Deleted role entity
        /// </returns>
        public Role DeleteRole(Guid id, int version, bool throwOnPopulatedRole)
        {
            var role = repository
                .AsQueryable<Role>(r => id == r.Id)
                .FetchMany(r => r.UserRoles)
                .ToList()
                .FirstOne();
            role.Version = version;

            DeleteRole(role, throwOnPopulatedRole);

            return role;
        }

        /// <summary>
        /// Saves the role.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// Saved role entity
        /// </returns>
        private Role SaveRole(Guid id, int version, string name, string description)
        {
            // Check if such role doesn't exist
            ValidateRoleName(id, name);

            Role role;
            if (!id.HasDefaultValue())
            {
                role = repository.AsQueryable<Role>(f => f.Id == id).FirstOne();

                if (role.IsSystematic)
                {
                    var logMessage = string.Format("Cannot save systematic role: {0} {1}", role.Name, role.Description);
                    var message = string.Format(UsersGlobalization.SaveRole_Cannot_Save_Systematic_Role, role.Description ?? role.Name);

                    throw new ValidationException(() => message, logMessage);
                }

                role.Version = version;
            }
            else
            {
                role = new Role();
            }

            role.Name = name;
            role.Description = description;

            repository.Save(role);

            return role;
        }

        /// <summary>
        /// Validates the name of the role.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        private void ValidateRoleName(Guid id, string name)
        {
            var query = repository
                .AsQueryable<Role>(r => r.Name == name.Trim());

            if (!id.HasDefaultValue())
            {
                query = query.Where(r => r.Id != id);
            }

            var existIngId = query
                .Select(r => r.Id)
                .FirstOrDefault();

            if (!existIngId.HasDefaultValue())
            {
                var message = string.Format(UsersGlobalization.SaveRole_RoleExists_Message, name);
                var logMessage = string.Format("Role already exists. Role name: {0}, Id: {1}", name, id);

                throw new ValidationException(() => message, logMessage);
            }
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if role has one or more members and do not delete role.</param>
        private void DeleteRole(Role role, bool throwOnPopulatedRole)
        {
            if (throwOnPopulatedRole && role.UserRoles.Any())
            {
                var logMessage = string.Format("Cannot delete populated role: {0} {1}", role.Name, role.Description);
                var message = string.Format(UsersGlobalization.DeleteRole_Cannot_Delete_Populated_Role, role.Description ?? role.Name);

                throw new ValidationException(() => message, logMessage);
            }

            if (role.IsSystematic)
            {
                var logMessage = string.Format("Cannot delete systematic role: {0} {1}", role.Name, role.Description);
                var message = string.Format(UsersGlobalization.DeleteRole_Cannot_Delete_Systematic_Role, role.Description ?? role.Name);

                throw new ValidationException(() => message, logMessage);
            }

            repository.Delete(role);

            foreach (var userRole in role.UserRoles)
            {
                repository.Delete(userRole);
            }
        }
    }
}