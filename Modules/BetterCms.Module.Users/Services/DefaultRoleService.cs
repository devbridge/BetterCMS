using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Models;

namespace BetterCms.Module.Users.Services
{
    public class DefaultRoleService : IRoleService
    {
        private readonly IRepository repository;

        public DefaultRoleService(IRepository repository)
        {
            this.repository = repository;
        }

        public Role CreateRole(string name)
        {
            return SaveRole(Guid.Empty, 1, name);
        }

        public Role UpdateRole(Guid id, int version, string name)
        {
            return SaveRole(id, version, name);
        }

        private Role SaveRole(Guid id, int version, string name)
        {
            // Check if such role doesn't exist
            ValidateRoleName(id, name);

            Role role;
            if (!id.HasDefaultValue())
            {
                role = repository.AsQueryable<Role>(f => f.Id == id).FirstOne();

                if (role.IsSystematic)
                {
                    var logMessage = string.Format("Cannot save systematic role: {0} {1}", role.Name, role.DisplayName);
                    var message = string.Format(UsersGlobalization.SaveRole_Cannot_Save_Systematic_Role, role.DisplayName ?? role.Name);

                    throw new ValidationException(() => message, logMessage);
                }

                role.Version = version;
            }
            else
            {
                role = new Role();
            }

            role.Name = name;

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
    }
}