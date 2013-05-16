using System;
using System.Collections.Generic;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Users.Models;

namespace BetterCms.Module.Users.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// Gets the list of user roles lookup values.
        /// </summary>
        /// <returns>List of user roles lookup values.</returns>
        IEnumerable<LookupKeyValue> GetUserRoles();

        /// <summary>
        /// Gets the role by id.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <returns>Role entity</returns>
        Role GetRole(Guid? id);
    }
}