namespace BetterCms.Module.Users.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// Created role entity
        /// </returns>
        Models.Role CreateRole(string name, string description = null);

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
        Models.Role UpdateRole(System.Guid id, int version, string name, string description = null);

        /// <summary>
        /// Saves the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="newEntityCreated">if set to <c>true</c> new entity was created.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> create if not exists.</param>
        /// <returns>
        /// Saved role entity
        /// </returns>
        Models.Role SaveRole(System.Guid id, int version, string name, string description, out bool newEntityCreated, bool createIfNotExists = false);

        /// <summary>
        /// Deletes the role by specified role id and version.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if role has one or more members and do not delete role.</param>
        /// <returns>
        /// Deleted role entity
        /// </returns>
        Models.Role DeleteRole(System.Guid id, int version, bool throwOnPopulatedRole);

        /// <summary>
        /// Deletes the role by specified role name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if role has one or more members and do not delete role.</param>
        /// <returns></returns>
        Models.Role DeleteRole(string name, bool throwOnPopulatedRole);
    }
}